using ESIndexingService.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ESIndexingService.Indexers
{
    public class FolderIndexer : BaseIndexer
    {
        private List<FolderRole> folderRoles;        

        public FolderIndexer(string esUrl, string indexName, List<FolderRole> folderRoles)  
            : base(esUrl, indexName)
        {
            this.folderRoles = folderRoles
                .Where(fr => fr.Type == SourceType.SharedFolder)
                .ToList();
        }

        public async Task Index()
        {
            foreach(FolderRole folderRole in this.folderRoles)
            {
                var directory = new DirectoryInfo(folderRole.Path);
                if (!directory.Exists)
                {
                    throw new Exception($"Folder {directory} not found!");
                }

                await IndexFolder(directory, folderRole.Roles);
            }
        }

        private async Task IndexFolder(DirectoryInfo directory, string[] roles)
        {
            foreach (var file in directory.GetFiles())
            {
                await this.IndexFile(file, roles);
            }

            foreach (var dir in directory.GetDirectories())
            {
                await this.IndexFolder(dir, roles);
            }
        }

        private async Task IndexFile(FileInfo file, string[] roles)
        {
            var docId = GetDocId(SourceType.SharedFolder, file.FullName);

            if(!await this.IsDocumentNewOrUpdated(docId, roles, file.LastWriteTime))
            {
                return;
            }

            var contents = this.textExtractor.Extract(file.FullName);

            var doc = new Document()
            {
                Id = docId,
                FileName = file.Name,
                Type = GetTypeFromFileName(file.Name),
                Source = SourceType.SharedFolder,
                SourceId = file.FullName,
                Content = contents.Text,
                Roles = roles,
                LastUpdated = file.LastWriteTime
            };
            await this.esClient.IndexDocumentAsync(doc);
        }
    }
}
