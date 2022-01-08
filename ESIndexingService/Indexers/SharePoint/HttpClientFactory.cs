using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ESIndexingService.SharePoint
{
    public class HttpClientFactory
    {
        public static async Task<HttpClient> GetAuthorizedHttpClient(string tenantId, string clientId, string clientSecret)
        {
            var values = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret),
                new KeyValuePair<string, string>("scope", "Files.ReadWrite.All"),
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("resource", "https://graph.microsoft.com")
            };

            var requestUrl = $"https://login.microsoftonline.com/{tenantId}/oauth2/token";
            var requestContent = new FormUrlEncodedContent(values);

            var client = new HttpClient();
            var response = await client.PostAsync(requestUrl, requestContent);
            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to get access token: {response.StatusCode} {message}");
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            dynamic tokenResponse = JsonConvert.DeserializeObject(responseBody);
            if (tokenResponse == null || tokenResponse.access_token == null)
            {
                throw new Exception($"Access token not found: {responseBody}");
            }

            var token = tokenResponse.access_token;

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            return client;
        }
    }
}
