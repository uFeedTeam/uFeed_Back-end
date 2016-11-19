using uFeed.DAL.SocialService.Models.FacebookModel.Common.Picture;

namespace uFeed.DAL.SocialService.Models.FacebookModel.Feed
{
    public class SerializedFeed
    {
        public string Id { get; set; } //group id

        public string Name { get; set; } //group name

        public Picture Picture { get; set; }

        public string Link { get; set; }

        public PostsCollection Feed { get; set; }
    }
}