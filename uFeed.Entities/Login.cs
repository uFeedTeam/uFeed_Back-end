using uFeed.Entities.Enums;

namespace uFeed.Entities
{
    public class Login : BaseType
    {
        public int ClientProfileId { get; set; }

        public Socials LoginType { get; set; }
    }
}
