using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ESIndexingService.SharePoint
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

        public async Task<string> ListChildren(string path)
        {
            var requestUrl = $"{baseUrl}/root:/{path}:/children";
            var response = await client.GetAsync(requestUrl);
            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to ListChildren of {path} : {response.StatusCode} {message}");
            }

            return await response.Content.ReadAsStringAsync();
        }
    }
}
