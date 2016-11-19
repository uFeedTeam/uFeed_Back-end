using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace uFeed.WEB.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(60, MinimumLength = 1, ErrorMessage = "String length has to be in range (1, 60)")]
        [RegularExpression(@"^\S.*$", ErrorMessage = "Name hasn't to start with whitespaces")]
        public string Name { get; set; }

        [Required]
        public int UserId { get; set; }

        public ClientProfileViewModel User { get; set; }

        public ICollection<SocialAuthorViewModel> Authors { get; set; }
    }
}
