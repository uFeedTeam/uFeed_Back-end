using uFeed.BLL.Enums;

namespace uFeed.BLL.DTO.Social
{
    public class AuthorDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public Socials Source { get; set; }

        public PhotoDTO Photo { get; set; }

    }
}