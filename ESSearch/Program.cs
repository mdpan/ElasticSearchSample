using ESIndexingService.Models;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace ESSearch
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var esUrl = ConfigurationManager.AppSettings["ElasticSearchURL"];
            var indexName = ConfigurationManager.AppSettings["IndexName"];

            SearchService service = new SearchService(esUrl, indexName);

            while (true)
            {
                Console.WriteLine("");
                Console.WriteLine("Enter search text (type 'exit' to exit)");
                var searchText = Console.ReadLine();

                if (searchText == "exit")
                {
                    break; // exit while loop
                }

                Console.WriteLine("");
                Console.WriteLine("Enter file type: 0=Doc, 1=Excel, 2=PPT, 3=PDF, 4=Text, 5=Others, press 'enter' returns files of all types; ");
                var fileTypeTxt = Console.ReadLine();

                FileType? fileType = null;
                try
                {
                    fileType = (FileType)int.Parse(fileTypeTxt);
                }
                catch
                {
                }

                var results = await service.Search(searchText, SourceType.SharedFolder, fileType, new[] { "role1", "role2" });

                if (results.Count <= 0)
                {
                    Console.WriteLine("No results found");
                    continue;
                }

                foreach (var item in results)
                {
                    Console.WriteLine("-----------");
                    Console.WriteLine($"Name: {item.FileName} | Type: {item.Type} | Source: {item.Source} | Last Updated: {item.LastUpdated}");
                    Console.WriteLine(item.Highlight);
                    Console.WriteLine("");
                }
            }
        }
    }
}
