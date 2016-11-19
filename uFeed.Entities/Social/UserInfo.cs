using uFeed.Entities.Enums;

namespace uFeed.Entities.Social
{
    public class UserInfo
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public Socials Source { get; set; }

        public Photo Photo { get; set; }
    }
}