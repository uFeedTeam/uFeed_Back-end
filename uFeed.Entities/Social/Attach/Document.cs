namespace uFeed.Entities.Social.Attach
{
    public class Document
    {
        public string Url { get; set; }

        public long Size { get; set; }

        public string Extension { get; set; }

        public Photo Photo { get; set; }
    }
}