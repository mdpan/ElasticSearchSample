using ESIndexingService.Models;
using System;

namespace ESSearch
{
    public class SearchResultItem
    {
        public string FileName { get; set; }

        public FileType Type { get; set; }

        public SourceType Source { get; set; }

        public string SourceId { get; set; }

        public string Highlight { get; set; }

        public string[] Roles { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
