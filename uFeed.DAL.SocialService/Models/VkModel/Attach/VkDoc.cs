using System.Runtime.Serialization;

namespace uFeed.DAL.SocialService.Models.VkModel.Attach
{
    [DataContract]
    public class VkDoc
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "owner_id")]
        public string OwnerId { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "size")]
        public long Size { get; set; }

        [DataMember(Name = "ext")]
        public string Ext { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "date")]
        public long Date { get; set; }

        [DataMember(Name = "type")]
        public long Type { get; set; }

        [DataMember(Name = "preview")]
        public VkPreview Preview { get; set; }
    }
}