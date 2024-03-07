using Infrastructure.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models.Auth
{
    public class SignUpFormModel
    {
        [Display(Name = "First name", Prompt = "Enter your first name", Order = 0)]
        [Required(ErrorMessage = "First name is required")]
        [MinLength(2, ErrorMessage = "First name is required")]
        public string FirstName { get; set; } = null!;

        [Display(Name = "Last name", Prompt = "Enter your last name", Order = 1)]
        [Required(ErrorMessage = "Last name is required")]
        [MinLength(2, ErrorMessage = "Last name is required")]
        public string LastName { get; set; } = null!;

        [Display(Name = "Email address", Prompt = "Enter your email address", Order = 2)]
        [Required(ErrorMessage = "Email address is required.")]
        [RegularExpression("^[^@\\s]+@[^@\\s]+\\.[^@\\s]{2,}$", ErrorMessage = "Invalid email address.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Display(Name = "Password", Prompt = "Enter your password", Order = 3)]
        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{8,}$", ErrorMessage = "Invalid password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Display(Name = "Confirm password", Prompt = "Confirm your password", Order = 4)]
        [Required(ErrorMessage = "You must confirm the password.")]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = null!;

        [Display(Name = "I agree to the Terms & Conditions", Order = 5)]
        [CheckBoxRequired(ErrorMessage = "You must accept the Terms & Conditions.")]
        [Required(ErrorMessage = "You must accept the Terms & Conditions.")]
        public bool Terms { get; set; } = false;
    }
}
