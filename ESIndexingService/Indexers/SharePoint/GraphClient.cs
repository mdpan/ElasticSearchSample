using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ESIndexingService.Indexers.SharePoint
{
    public class GraphClient
    {
        private HttpClient client;
        private string baseUrl;

        public GraphClient(HttpClient client, string siteId)
        {
            this.client = client;
            this.baseUrl = $"https://graph.microsoft.com/beta/sites/{siteId}";
        }

        public async Task<List<DriveItem>> ListChildren(string path)
        {
            var requestUrl = $"{baseUrl}/drive/root:/{path}:/children";
            var response = await client.GetAsync(requestUrl);
            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to ListChildren of {path} : {response.StatusCode} {message}");
            }

            var responseString = await response.Content.ReadAsStringAsync();

            dynamic responseObject = JsonConvert.DeserializeObject(responseString);
            
            var driveItems = new List<DriveItem>();
            foreach(var item in responseObject.value)
            {
                var driveItem = new DriveItem
                {
                    Id = item.id,
                    Name = item.name,
                    LastModified = item.lastModifiedDateTime,
                };

                if (item.folder != null )
                {
                    driveItem.Type = DriveItemType.Folder;
                }

                if (item.file != null)
                {
                    driveItem.Type = DriveItemType.File;
                }

                driveItems.Add(driveItem);
            }

            return driveItems;
        }

        public async Task<byte[]> DownloadFile(string fileId)
        {
            var requestUrl = $"{baseUrl}/drive/items/{fileId}/content";
            var response = await client.GetAsync(requestUrl);
            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to download pdf file: {response.StatusCode} {message}");
            }

            return await response.Content.ReadAsByteArrayAsync();
        }
    }
}
