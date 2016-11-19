using System.ComponentModel.DataAnnotations;

namespace uFeed.WEB.ViewModels
{
    public class SocialAuthorViewModel
    {
        public int Id { get; set; }

        [Required]
        public string AuthorId { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}
