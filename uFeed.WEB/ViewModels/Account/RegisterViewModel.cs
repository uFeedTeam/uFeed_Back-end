using System.ComponentModel.DataAnnotations;

namespace uFeed.WEB.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email is required")]
        [StringLength(65, ErrorMessage = "Invalid email length")]
        [RegularExpression(@"^\S+@\S+\.\S+$", ErrorMessage = "Invalid email")]
        public string Email { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required")]
        [RegularExpression(@"^\S.*$", ErrorMessage = "Invalid name")]
        public string Name { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Invalid password length")]
        [RegularExpression(@"(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{6,20})$", ErrorMessage = "Password must contain small and big letters, digits.")]
        public string Password { get; set; }

        [Display(Name = "ConfirmPassword")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "ConfirmPassword is required")]
        [Compare("Password", ErrorMessage = "Doesn` equal to password")]
        public string ConfirmPassword { get; set; }
    }
}