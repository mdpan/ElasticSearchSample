using ESIndexingService.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TikaOnDotNet.TextExtraction;

namespace ESIndexingService.Indexers
{
    public abstract class BaseIndexer
    {
        protected readonly ElasticClient esClient;
        protected readonly TextExtractor textExtractor;

        protected BaseIndexer(string esUrl, string indexName)
        {
            var settings = new ConnectionSettings(new Uri(esUrl))
               .DefaultIndex(indexName);

            this.esClient = new ElasticClient(settings);

            this.CreateIndex(indexName);

            // Use TikaOnDotNet to extract the contents of the document.
            this.textExtractor = new TextExtractor();
        }

        protected async Task<bool> IsDocumentNewOrUpdated(string docId, string[] roles, DateTime lastUpdatedTime)
        {
            var docPath = new DocumentPath<Document>(docId);
            var response = await this.esClient.GetAsync<Document>(docPath);

            if(!response.Found)
            {
                return true;
            }

            var doc = response.Source;
                        
            if (doc.LastUpdated != lastUpdatedTime)
            {
                return true;
            }

            if(!doc.Roles.OrderBy(x => x).SequenceEqual(roles.OrderBy(x => x)))
            {
                return true;
            }

            return false;
        }

        protected static string GetDocId(SourceType sourceType, string sourceId)
        {
            using (SHA256 sha = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                return System.Convert.ToBase64String(sha.ComputeHash(enc.GetBytes($"{sourceType}{sourceId}")));
            }
        }

        protected static FileType GetTypeFromFileName(string fileName)
        {
            var fileInfo = new FileInfo(fileName);
            var extension = fileInfo.Extension.ToLower();

            switch (extension)
            {
                case ".doc":
                case ".docx":
                    return FileType.DOC;
                case ".xls":
                case ".xlsx":
                    return FileType.XLS;
                case ".ppt":
                case ".pptx":
                    return FileType.PPT;
                case ".pdf":
                    return FileType.PDF;
                case ".txt":
                    return FileType.TXT;
                default:
                    return FileType.Others;
            }
        }

        private void CreateIndex(string indexName)
        {
            if (this.esClient.Indices.Exists(indexName).Exists)
            {
                return;
            }

            var kuromojiTokenizer = new KuromojiTokenizer();
            kuromojiTokenizer.Mode = KuromojiTokenizationMode.Search;

            var customAnalyzer = new CustomAnalyzer();
            customAnalyzer.Tokenizer = kuromojiTokenizer.Type;
            customAnalyzer.Filter = new List<string>()
            {
                "kuromoji_baseform",
                "kuromoji_part_of_speech",
                "cjk_width",
                "ja_stop",
                "kuromoji_stemmer",
                "lowercase"
            };
            customAnalyzer.CharFilter = new List<string>()
            {
                 "icu_normalizer"
            };

            var indexSettings = new IndexSettings();
            indexSettings.Analysis = new Analysis();
            indexSettings.Analysis.Tokenizers = new Tokenizers();
            indexSettings.Analysis.Tokenizers.Add(kuromojiTokenizer.Type, kuromojiTokenizer);
            indexSettings.Analysis.Analyzers = new Analyzers();
            indexSettings.Analysis.Analyzers.Add("kuromoji_normalize", customAnalyzer);

            var indexConfig = new IndexState
            {
                Settings = indexSettings
            };

            if(this.esClient.Indices.Exists(indexName).Exists)
            {
                return;
            }

            var createIndexResponse = this.esClient.Indices.Create(
                indexName,
                c => c.InitializeUsing(indexConfig).Map<Document>(
                    m => m.Properties(p => p.Text(
                        t => t.Name(n => n.Content).Analyzer("kuromoji_normalize")
                    ))
                )
            );

            if(!createIndexResponse.IsValid)
            {
                throw new Exception(createIndexResponse.DebugInformation);
            }
        }
    }
}
