using System.Runtime.Serialization;

namespace uFeed.DAL.SocialService.Models.VkModel
{
    [DataContract]
    public class VkGroup
    {
        [DataMember(Name = "gid")]
        public string Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "screen_name")]
        public string ScreenName { get; set; }

        [DataMember(Name = "is_closed")]
        public int IsClosed { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "photo")]
        public string Photo { get; set; }

        [DataMember(Name = "photo_medium")]
        public string PhotoMedium { get; set; }

        [DataMember(Name = "photo_big")]
        public string PhotoBig { get; set; }
    }
}