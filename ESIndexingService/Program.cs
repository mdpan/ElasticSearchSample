using ESIndexingService.Indexers;
using ESIndexingService.Models;
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

            var esUrl = ConfigurationManager.AppSettings["ElasticSearchURL"];
            var indexName = ConfigurationManager.AppSettings["IndexName"];
            var folderRoles = GetFolderRoles();            

            var folderIndexer = new FolderIndexer(esUrl, indexName, folderRoles);

            while (true)
            {
                Console.WriteLine($"Indexing Shared folder started at {DateTime.Now.ToString("HH:mm:ss")}");
                await folderIndexer.Index();
                Thread.Sleep(10000); // Sleep 10 seconds
            }
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
