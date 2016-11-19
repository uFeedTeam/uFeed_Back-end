using System.Collections.Generic;

namespace uFeed.DAL.SocialService.Models.FacebookModel.Feed
{
    public class Attachment
    {
        public AttachmentsCollection Subattachments { get; set; }
        public string Description { get; set; }
        public List<Description_tag> Description_tags { get; set; }
        public Media Media { get; set; }
        public Target Target { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }
}