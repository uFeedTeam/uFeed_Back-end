using uFeed.BLL.Enums;

namespace uFeed.BLL.DTO
{
    public class SocialAuthorDTO
    {
        public int Id { get; set; }

        public string AuthorId { get; set; }

        public int CategoryId { get; set; }

        public Socials Source { get; set; }
    }
}
