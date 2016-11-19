using System.Collections.Generic;
using System.Runtime.Serialization;

namespace uFeed.DAL.SocialService.Models.VkModel.Attach
{
    [DataContract]
    public class VkDocPhoto
    {
        [DataMember(Name = "sizes")]
        public List<VkDocPhotoSize> Sizes{ get; set; }
    }
}