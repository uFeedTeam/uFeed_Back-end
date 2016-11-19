namespace uFeed.BLL.DTO.Social.Attach
{
    public class AttachmentDTO
    {
        public string Type { get; set; }

        public string Title { get; set; }

        public PhotoDTO Photo { get; set; }

        public VideoDTO Video { get; set; }

        public AlbumDTO Album { get; set; }

        public DocumentDTO Document { get; set; }

        public LinkDTO Link { get; set; }

        public PollDTO Poll { get; set; }
        public PostDTO Repost { get; set; }
    }
}