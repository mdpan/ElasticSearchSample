using ESIndexingService.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ESSearch
{
    public class SearchService
    {
        private readonly ElasticClient client;

        public SearchService(string elasticSearchUrl, string indexName)
        {
            var settings = new ConnectionSettings(new Uri(elasticSearchUrl))
                .DefaultIndex(indexName);

            this.client = new ElasticClient(settings);
        }

        public async Task<List<SearchResultItem>> Search(
            string searchText,
            SourceType? sourceType = null,
            FileType? fileType = null,
            string[] roles = null,
            int from = 0,
            int size = 10
        )
        {
            QueryContainer ContentQuery(QueryContainerDescriptor<Document> q) =>
                q.MatchPhrase(m => m.Analyzer("kuromoji_normalize").Boost(1.1).Slop(2).Field(f => f.Content).Query(searchText));
            var query = ContentQuery(new QueryContainerDescriptor<Document>());

            if (sourceType != null)
            {
                QueryContainer SourceTypeQuery(QueryContainerDescriptor<Document> q) =>
                    q.Term(m => m.Source, sourceType);

                query &= SourceTypeQuery(new QueryContainerDescriptor<Document>());
            }

            if (fileType != null)
            {
                QueryContainer SourceTypeQuery(QueryContainerDescriptor<Document> q) =>
                    q.Term(m => m.Type, fileType);

                query &= SourceTypeQuery(new QueryContainerDescriptor<Document>());
            }

            if (roles != null)
            {
                QueryContainer RolesQuery(QueryContainerDescriptor<Document> q) =>
                    q.Terms(m => m.Field(f => f.Roles).Terms(roles));

                query &= RolesQuery(new QueryContainerDescriptor<Document>());
            }

            HighlightDescriptor<Document> Highlight(HighlightDescriptor<Document> h) =>
                h.Fields(fs => fs.Field(f => f.Content));

            var searchRequest = new SearchRequest<Document>
            {
                From = from,
                Size = size,
                Query = query,
                Highlight = Highlight(new HighlightDescriptor<Document>())
            };

            var response = await this.client.SearchAsync<Document>(searchRequest);

            var items = new List<SearchResultItem>();

            foreach (var hit in response.Hits)
            {
                foreach (var highlight in hit.Highlight)
                {
                    foreach (var highlightItem in highlight.Value)
                    {
                        if (items.Count > 50)
                            return items;

                        items.Add(new SearchResultItem()
                        {
                            FileName = hit.Source.FileName,
                            Type = hit.Source.Type,
                            Source = hit.Source.Source,
                            SourceId = hit.Source.SourceId,
                            Highlight = highlightItem,
                            Roles = hit.Source.Roles,
                            LastUpdated = hit.Source.LastUpdated
                        });
                    }
                }
            }

            return items;
        }
    }
}
