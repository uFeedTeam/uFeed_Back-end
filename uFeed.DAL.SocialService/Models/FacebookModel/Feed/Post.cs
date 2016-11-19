namespace uFeed.DAL.SocialService.Models.FacebookModel.Feed
{
    public class Post
    {
        public string Message { get; set; }
        public string Link { get; set; }
        public string Name { get; set; }
        public string Created_time { get; set; }
        public string Id { get; set; }
        public Likes Likes { get; set; }
        public AttachmentsCollection Attachments { get; set; }
    }
}