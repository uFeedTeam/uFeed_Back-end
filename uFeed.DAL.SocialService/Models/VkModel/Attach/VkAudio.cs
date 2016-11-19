using System.Runtime.Serialization;

namespace uFeed.DAL.SocialService.Models.VkModel.Attach
{
    [DataContract]
    public class VkAudio
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "owner_id")]
        public string OwnerId { get; set; }

        [DataMember(Name = "artist")]
        public string Artist { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "duration")]
        public long Duration { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "lyrics_id")]
        public string LyricsId { get; set; }

        [DataMember(Name = "album_id")]
        public string AlbumId { get; set; }

        [DataMember(Name = "genre_id")]
        public string GenreId { get; set; }

        [DataMember(Name = "date")]
        public long Date { get; set; }

        [DataMember(Name = "no_search")]
        public bool NoSearch { get; set; }
    }
}