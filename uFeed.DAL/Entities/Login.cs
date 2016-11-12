using uFeed.DAL.Enums;

namespace uFeed.DAL.Entities
{
    public class Login : BaseType
    {
        public int ClientProfileId { get; set; }

        public Socials LoginType { get; set; }
    }
}
