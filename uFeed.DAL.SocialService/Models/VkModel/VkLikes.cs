using System.Runtime.Serialization;

namespace uFeed.DAL.SocialService.Models.VkModel
{
    [DataContract]
    public class VkLikes
    {
        [DataMember(Name = "count")]
        public long Count { get; set; }

        [DataMember(Name = "user_likes")]
        public int UserLikes { get; set; }

        [DataMember(Name = "can_like")]
        public int CanLike { get; set; }

        [DataMember(Name = "can_publish")]
        public int CanPublish { get; set; }
    }
}