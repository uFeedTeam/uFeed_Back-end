using System;
using System.Collections.Generic;
using uFeed.WEB.ViewModels.Social.Attach;

namespace uFeed.WEB.ViewModels.Social
{
    public class PostViewModel : IComparable
    {
        public string Id { get; set; }

        public DateTime CreatedTime { get; set; }

        public string Message { get; set; }

        public string Url { get; set; }

        public AuthorViewModel Author { get; set; }

        public LikesViewModel Likes { get; set; }

        public List<AttachmentViewModel> Attachments { get; set; }

        public int CompareTo(object obj)
        {
            return ((PostViewModel) obj).CreatedTime.CompareTo(CreatedTime);
        }
    }
}