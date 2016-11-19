using uFeed.Entities.Enums;

namespace uFeed.BLL.DTO.Social
{
    public class UserInfoDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public Socials Source { get; set; }

        public PhotoDTO Photo { get; set; }
    }
}