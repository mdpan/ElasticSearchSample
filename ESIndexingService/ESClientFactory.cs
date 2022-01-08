using ESIndexingService.Models;
using Nest;
using System;
using System.Collections.Generic;

namespace ESIndexingService
{
    public class ESClientFactory
    {
        public static ElasticClient GeteESclient(string esUrl, string indexName)
        {
            var settings = new ConnectionSettings(new Uri(esUrl))
               .DefaultIndex(indexName);

            var esClient = new ElasticClient(settings);

            CreateIndex(esClient, indexName); 

            return esClient;
        }

        private static void CreateIndex(ElasticClient esClient, string indexName)
        {
            if (esClient.Indices.Exists(indexName).Exists)
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

            if (esClient.Indices.Exists(indexName).Exists)
            {
                return;
            }

            var createIndexResponse = esClient.Indices.Create(
                indexName,
                c => c.InitializeUsing(indexConfig).Map<Document>(
                    m => m.Properties(p => p.Text(
                        t => t.Name(n => n.Content).Analyzer("kuromoji_normalize")
                    ))
                )
            );

            if (!createIndexResponse.IsValid)
            {
                throw new Exception(createIndexResponse.DebugInformation);
            }
        }
    }
}
