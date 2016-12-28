using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using uFeed.BLL.Enums;

namespace uFeed.WEB.ViewModels
{
    public class UserViewModel
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

        public ICollection<Socials> Logins { get; set; }

        public byte[] Photo { get; set; }

        public ICollection<CategoryViewModel> Categories { get; set; }
    }
}
