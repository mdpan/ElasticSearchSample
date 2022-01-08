using ESIndexingService.Indexers.SharePoint;
using ESIndexingService.Models;
using Nest;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESIndexingService.Indexers
{
    public class SharePointIndexer : BaseIndexer
    {
        private List<FolderRole> folderRoles;

        private GraphClient graphClient;

        public SharePointIndexer(ElasticClient esClient, List<FolderRole> folderRoles, GraphClient graphClient)
            : base(esClient)
        {
            this.folderRoles = folderRoles
                .Where(fr => fr.Type == SourceType.SharePoint)
                .ToList();

            this.graphClient = graphClient;

        }

        public async Task Index()
        {
            foreach (FolderRole folderRole in this.folderRoles)
            {
                await IndexFolder(folderRole.Path, folderRole.Roles);
            }
        }

        private async Task IndexFolder(string path, string[] roles)
        {
            var driveItems = await this.graphClient.ListChildren(path);

            foreach (var file in driveItems.Where(i => i.Type == DriveItemType.File))
            {
                await IndexFile(file, roles);
            }

            foreach (var folder in driveItems.Where(i => i.Type == DriveItemType.Folder))
            {
                await IndexFolder(path + "/" + folder.Name, roles);
            }
        }

        private async Task IndexFile(DriveItem file, string[] roles)
        {
            var docId = GetDocId(SourceType.SharePoint, file.Id);

            if (!await this.IsDocumentNewOrUpdated(docId, roles, file.LastModified))
            {
                return;
            }

            var fileBytes = await this.graphClient.DownloadFile(file.Id);

            var contents = this.textExtractor.Extract(fileBytes);

            var doc = new Document()
            {
                Id = docId,
                FileName = file.Name,
                Type = GetTypeFromFileName(file.Name),
                Source = SourceType.SharePoint,
                SourceId = file.Id,
                Content = contents.Text,
                Roles = roles,
                LastUpdated = file.LastModified
            };
            await this.esClient.IndexDocumentAsync(doc);
        }
    }
}
