using System.Runtime.Serialization;

namespace uFeed.DAL.SocialService.Models.VkModel.Attach
{
    [DataContract]
    public class VkAttachment
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "photo")]
        public VkPhoto Photo { get; set; }

        [DataMember(Name = "link")]
        public VkLink Link { get; set; }

        [DataMember(Name = "doc")]
        public VkDoc Doc { get; set; }

        [DataMember(Name = "video")]
        public VkVideo Video { get; set; }

        [DataMember(Name = "audio")]
        public VkAudio Audio { get; set; }

        [DataMember(Name = "poll")]
        public VkPoll Poll { get; set; }
    }
}