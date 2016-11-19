using System.Collections.Generic;
using uFeed.DAL.SocialService.Models.FacebookModel.Common;

namespace uFeed.DAL.SocialService.Models.FacebookModel.Feed
{
    public class PostsCollection
    {
        public List<Post> Data { get; set; }
        public Paging Paging { get; set; }
    }
}