using System;
using uFeed.DAL.SocialService.Models.VkModel.Attach;
using System.Runtime.Serialization;

namespace uFeed.DAL.SocialService.Models.VkModel
{
    [DataContract]
    public class VkWallPost : IComparable
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "owner_id ")]
        public string OwnerId { get; set; }

        [DataMember(Name = "from_id")]
        public string FromId { get; set; }

        [DataMember(Name = "date")]
        public long Date { get; set; }

        [DataMember(Name = "text")]
        public string Text { get; set; }

        [DataMember(Name = "post_type")]
        public string PostType { get; set; }

        [DataMember(Name = "is_pinned")]
        public int IsPinned { get; set; }

        [DataMember(Name = "attachments")]
        public VkAttachment[] Attachments { get; set; }

        [DataMember(Name = "likes")]
        public VkLikes Likes { get; set; }

        public int CompareTo(object obj)
        {
            return ((VkWallPost)obj).Date.CompareTo(Date);
        }
    }
}