using ESIndexingService.Indexers.Database;
using ESIndexingService.Models;
using Nest;
using System.Threading.Tasks;
using UPubPlat.EduKoumu.Functions;

namespace ESIndexingService.Indexers
{
    public class DBIndexer : BaseIndexer
    {
        private DBClient dbClient;

        public DBIndexer(ElasticClient esClient)
            : base(esClient)
        {
            this.dbClient = new DBClient();
        }

        public async Task Index()
        {
            foreach (var subDoc in this.dbClient.GetPdfSubDocs())
            {
                await this.IndexFile(subDoc);
            }
        }

        private async Task IndexFile(PdfSubDocModel subDoc)
        {
            if(subDoc.PdfData == null)
            {
                return;
            }

            if(subDoc.UpdDateTime == null)
            {
                return;
            }

            var docId = GetDocId(SourceType.Database, subDoc.PdfDocSubId);

            var roles = this.dbClient.GetUserRoles(subDoc.CrtUserId.Value);

            if (!await this.IsDocumentNewOrUpdated(docId, roles, subDoc.UpdDateTime.Value))
            {
                return;
            }

            var contents = this.textExtractor.Extract(subDoc.PdfData);

            var doc = new Document()
            {
                Id = docId,
                FileName = subDoc.FileName,
                Type = GetTypeFromFileName(subDoc.FileName),
                Source = SourceType.Database,
                SourceId = subDoc.PdfDocSubId,
                Content = contents.Text,
                Roles = roles,
                LastUpdated = subDoc.UpdDateTime.Value
            };
            await this.esClient.IndexDocumentAsync(doc);
        }
    }
}
