using System.Runtime.Serialization;

namespace uFeed.DAL.SocialService.Models.VkModel.Attach
{
    [DataContract]
    public class VkPhoto
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "album_id")]
        public string AlbumId { get; set; }

        [DataMember(Name = "owner_id")]
        public string OwnerId { get; set; }

        [DataMember(Name = "user_id")]
        public string UserId { get; set; }

        [DataMember(Name = "text")]
        public string Text { get; set; }

        [DataMember(Name = "date")]
        public long Date { get; set; }

        [DataMember(Name = "photo_75")]
        public string Photo75 { get; set; }

        [DataMember(Name = "photo_130")]
        public string Photo130 { get; set; }

        [DataMember(Name = "photo_604")]
        public string Photo604 { get; set; }

        [DataMember(Name = "photo_807")]
        public string Photo807 { get; set; }

        [DataMember(Name = "photo_1280")]
        public string Photo1280 { get; set; }

        [DataMember(Name = "photo_2560")]
        public string Photo2560 { get; set; }

        [DataMember(Name = "width")]
        public string Width { get; set; }

        [DataMember(Name = "height")]
        public string Height { get; set; }
    }
}