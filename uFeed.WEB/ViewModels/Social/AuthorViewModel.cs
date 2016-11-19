using uFeed.BLL.Enums;

namespace uFeed.WEB.ViewModels.Social
{
    public class AuthorViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public Socials Source { get; set; }

        public PhotoViewModel Photo { get; set; }

    }
}