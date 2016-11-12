namespace uFeed.WEB.ViewModels.Social.Attach
{
    public class AttachmentViewModel
    {
        public string Type { get; set; }

        public string Title { get; set; }

        public PhotoViewModel Photo { get; set; }

        public VideoViewModel Video { get; set; }

        public AlbumViewModel Album { get; set; }

        public DocumentViewModel Document { get; set; }

        public LinkViewModel Link { get; set; }

        public PollViewModel Poll { get; set; }
        public PostViewModel Repost { get; set; }
    }
}