using uFeed.DAL.Entities.Social;

namespace uFeed.DAL.Entities.Attach
{
    public class Attachment
    {
        public string Type { get; set; }

        public string Title { get; set; }

        public Photo Photo { get; set; }

        public Video Video { get; set; }

        public Album Album { get; set; }

        public Document Document { get; set; }

        public Link Link { get; set; }

        public Poll Poll { get; set; }

        public Post Repost { get; set; }
    }
}