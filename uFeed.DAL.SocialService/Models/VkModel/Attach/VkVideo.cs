using System.Runtime.Serialization;

namespace uFeed.DAL.SocialService.Models.VkModel.Attach
{

    [DataContract]
    public class VkVideo
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "owner_id")]
        public string OwnerId { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "duration")]
        public long Duration { get; set; }

        [DataMember(Name = "photo_130")]
        public string Photo130 { get; set; }

        [DataMember(Name = "photo_320")]
        public string Photo320 { get; set; }

        [DataMember(Name = "photo_640")]
        public string Photo640 { get; set; }

        [DataMember(Name = "date")]
        public long Date { get; set; }

        [DataMember(Name = "adding_date")]
        public long AddingDate { get; set; }

        [DataMember(Name = "views")]
        public int Views { get; set; }

        [DataMember(Name = "comments")]
        public long Comments { get; set; }

        [DataMember(Name = "player")]
        public string Player{ get; set; }

        [DataMember(Name = "access_key")]
        public string AccessKey { get; set; }

        [DataMember(Name = "processing")]
        public int Processing { get; set; }

        [DataMember(Name = "live")]
        public int Live { get; set; }
    }
}