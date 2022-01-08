using System;

namespace ESIndexingService.Indexers.SharePoint
{
    public class DriveItem
    {
        public string Id { get; set; }

        public DriveItemType Type { get; set; }

        public string Name { get; set; }

        public DateTime LastModified { get; set; }
    }
}
