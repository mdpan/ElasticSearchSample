namespace ESIndexingService.Models
{
    public class FolderRole
    {
        public string Path { get; set; }

        public SourceType Type { get; set; }

        public string[] Roles { get; set; }
    }
}
