using System.Runtime.Serialization;

namespace uFeed.DAL.SocialService.Models.VkModel.Attach
{
    [DataContract]
    public class VkPreview
    {
        [DataMember(Name = "photo")]
        public VkDocPhoto Photo { get; set; }

    }
}