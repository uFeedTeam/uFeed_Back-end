using System.Collections.Generic;

namespace uFeed.DAL.SocialService.Models.FacebookModel.Feed
{
    public class Likes
    {
        public List<Like> Data { get; set; }
        public LikesSummary Summary { get; set; }
    }
}