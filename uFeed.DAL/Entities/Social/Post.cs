using System;
using System.Collections.Generic;
using uFeed.DAL.Entities.Attach;

namespace uFeed.DAL.Entities.Social
{
    public class Post
    {
        public string Id { get; set; }

        public DateTime CreatedTime { get; set; }

        public string Message { get; set; }

        public string Url { get; set; }

        public Author Author { get; set; }

        public Likes Likes { get; set; }

        public List<Attachment> Attachments { get; set; }
    }
}