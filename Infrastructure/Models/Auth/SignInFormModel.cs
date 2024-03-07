using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models.Auth;

public class SignInFormModel
{
    [Display(Name = "Email address", Prompt = "Enter your email address", Order = 0)]
    [Required(ErrorMessage = "Email address is required.")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Display(Name = "Password", Prompt = "Enter your password", Order = 1)]
    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Display(Name = "Remember me", Order = 2)]
    public bool Remember { get; set; } = false;
}
