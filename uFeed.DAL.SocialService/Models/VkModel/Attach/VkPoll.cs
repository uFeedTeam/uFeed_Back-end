using System.Collections.Generic;
using System.Runtime.Serialization;

namespace uFeed.DAL.SocialService.Models.VkModel.Attach
{
    [DataContract]
    public class VkPoll
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "owner_id")]
        public string OwnerId { get; set; }

        [DataMember(Name = "created")]
        public string Created { get; set; }

        [DataMember(Name = "question")]
        public string Question { get; set; }

        [DataMember(Name = "votes")]
        public long Votes { get; set; }

        [DataMember(Name = "answer_id")]
        public string AnswerId { get; set; }

        [DataMember(Name = "answers")]
        public List<VkAnswer> Answers { get; set; }
    }
}