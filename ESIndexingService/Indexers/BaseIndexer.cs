using ESIndexingService.Models;
using Nest;
using System;
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

        protected BaseIndexer(ElasticClient esClient)
        {
            this.esClient = esClient;

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

    }
}
