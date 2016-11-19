using System.Collections.Generic;
using uFeed.WEB.ViewModels.Social;

namespace uFeed.WEB.ViewModels
{
    public class GetAuthorsViewModel
    {
        public IEnumerable<AuthorViewModel> VkAuthors { get; set; }
        public IEnumerable<AuthorViewModel> FacebookAuthors { get; set; }
    }
}