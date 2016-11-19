using uFeed.Entities.Enums;

namespace uFeed.Entities
{
    public class SocialAuthor : BaseType
    {
        public string AuthorId { get; set; }

        public int CategoryId { get; set; }

        public Socials Source { get; set; }
    }
}
