using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models.Account;

public class AccountSecurityPasswordModel
{
    [Display(Name = "Current password", Prompt = "Enter your current password.")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "You must enter your current password.")]
    public string OldPassword { get; set; } = null!;

    [Display(Name = "New password", Prompt = "Enter a new password.")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "You must enter a new password.")]
    [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{8,}$", ErrorMessage = "Invalid password")]
    public string NewPassword { get; set; } = null!;

    [Display(Name  = "Confirm password", Prompt = "Confirm your password.")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "You must confirm your new password.")]
    [Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match.")]
    public string ConfirmNewPassword { get; set; } = null!;

}
