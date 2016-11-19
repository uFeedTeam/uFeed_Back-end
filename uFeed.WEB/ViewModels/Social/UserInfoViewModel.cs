using uFeed.BLL.Enums;

namespace uFeed.WEB.ViewModels.Social
{
    public class UserInfoViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public Socials Source { get; set; }

        public PhotoViewModel Photo { get; set; }
    }
}