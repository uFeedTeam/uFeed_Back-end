using uFeed.BLL.Enums;

namespace uFeed.BLL.DTO
{
    public class ClientBookmarkDTO
    {
        public int Id { get; set; }

        public string PostId { get; set; }

        public Socials Source { get; set; }
    }
}