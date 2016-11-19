using System.Runtime.Serialization;

namespace uFeed.DAL.SocialService.Models.VkModel.Attach
{
    [DataContract]
    public class VkAnswer
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "text")]
        public string Text { get; set; }

        [DataMember(Name = "votes")]
        public long Votes{ get; set; }

        [DataMember(Name = "rate")]
        public double Rate { get; set; }
    }
}