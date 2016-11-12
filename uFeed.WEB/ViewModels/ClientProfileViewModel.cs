using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace uFeed.WEB.ViewModels
{
    public class ClientProfileViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(60, MinimumLength = 1, ErrorMessage = "String length has to be in range (1, 60)")]
        [RegularExpression(@"^\S.*$", ErrorMessage = "Name hasn't to start with whitespaces")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(60, MinimumLength = 1, ErrorMessage = "String length has to be in range (1, 60)")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public ICollection<CategoryViewModel> Categories { get; set; }
    }
}
