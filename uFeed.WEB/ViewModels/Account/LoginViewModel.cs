using System.ComponentModel.DataAnnotations;

namespace uFeed.WEB.ViewModels.Account
{
    public class LoginViewModel
    {
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        public bool IsPersistent { get; set; }
    }
}