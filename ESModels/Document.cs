using System;

namespace ESIndexingService.Models
{
    public class Document
    {
        public string Id { get; set; }

        public string FileName { get; set; }

        public FileType Type { get; set; }

        public SourceType Source { get; set; }

        public string SourceId { get; set; }

        public string Content { get; set; }

        public string[] Roles { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
