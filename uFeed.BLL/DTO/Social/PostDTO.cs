using System;
using System.Collections.Generic;
using uFeed.BLL.DTO.Social.Attach;

namespace uFeed.BLL.DTO.Social
{
    public class PostDTO : IComparable
    {
        public string Id { get; set; }

        public DateTime CreatedTime { get; set; }

        public string Message { get; set; }

        public string Url { get; set; }

        public AuthorDTO Author { get; set; }

        public LikesDTO Likes { get; set; }

        public List<AttachmentDTO> Attachments { get; set; }

        public int CompareTo(object obj)
        {
            return ((PostDTO) obj).CreatedTime.CompareTo(CreatedTime);
        }
    }
}