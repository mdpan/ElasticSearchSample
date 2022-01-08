using ESIndexingService.Models;
using ESIndexingService.SharePoint;
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
            foreach(FolderRole folderRole in this.folderRoles)
            {
                await IndexFolder(folderRole.Path, folderRole.Roles);
            }
        }

        private async Task IndexFolder(string path, string[] roles)
        {
            var result = await this.graphClient.ListChildren(path);
        }

    }
}
