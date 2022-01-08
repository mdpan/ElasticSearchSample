using ESIndexingService.Indexers;
using ESIndexingService.Models;
using ESIndexingService.SharePoint;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ESIndexingService
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Indexing Service started...");

            var esClient = GetElasticClient();
            var graphClient = await GetGraphClient();
            var folderRoles = GetFolderRoles();            

            var folderIndexer = new FolderIndexer(esClient, folderRoles);
            var sharepointIndexer = new SharePointIndexer(esClient, folderRoles, graphClient);

            while (true)
            {
                // Console.WriteLine($"Indexing Shared folder started at {DateTime.Now.ToString("HH:mm:ss")}");
                // await folderIndexer.Index();
                // Thread.Sleep(10000); // Sleep 10 seconds

                Console.WriteLine($"Indexing Sharepoint Server started at {DateTime.Now.ToString("HH:mm:ss")}");
                await sharepointIndexer.Index();
                Thread.Sleep(10000); // Sleep 10 seconds
            }
        }

        static ElasticClient GetElasticClient()
        {
            var esUrl = ConfigurationManager.AppSettings["ElasticSearchURL"];
            var indexName = ConfigurationManager.AppSettings["IndexName"];
            return ESClientFactory.GeteESclient(esUrl, indexName);
        }

        static async Task<GraphClient> GetGraphClient()
        {
            var tenantId = ConfigurationManager.AppSettings["SPTenantId"];
            var clientId = ConfigurationManager.AppSettings["SPClientId"];
            var clientSecret = ConfigurationManager.AppSettings["SPClientSecret"];
            var siteId = ConfigurationManager.AppSettings["SPSiteId"];

            var httpClient = await HttpClientFactory.GetAuthorizedHttpClient(tenantId, clientId, clientSecret);            

            return new GraphClient(httpClient, siteId);
        }

        static List<FolderRole> GetFolderRoles()
        {
            var folderRolesPath = Path.Combine(Environment.CurrentDirectory, "FolderRoles.json");
            var sr = new StreamReader(folderRolesPath);
            var json = sr.ReadToEnd();
            return JsonConvert.DeserializeObject<List<FolderRole>>(json);
        }
    }
}
