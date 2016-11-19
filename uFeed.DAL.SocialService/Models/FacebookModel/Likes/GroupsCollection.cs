using System.Collections.Generic;
using uFeed.DAL.SocialService.Models.FacebookModel.Common;

namespace uFeed.DAL.SocialService.Models.FacebookModel.Likes
{
    public class GroupsCollection
    {
        public List<Group> Data { set; get; }

        public Paging Paging { get; set; }
    }
}