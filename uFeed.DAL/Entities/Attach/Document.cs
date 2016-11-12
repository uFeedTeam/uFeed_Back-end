using uFeed.DAL.Entities.Social;

namespace uFeed.DAL.Entities.Attach
{
    public class Document
    {
        public string Url { get; set; }

        public long Size { get; set; }

        public string Extension { get; set; }

        public Photo Photo { get; set; }
    }
}