using System.ComponentModel.DataAnnotations;

namespace Silicon.Models;

public class AccountDetailsBasicFormModel
{
    [DataType(DataType.ImageUrl)]
    public string? ProfileImage { get; set; }

    [Display(Name = "First name", Prompt = "Enter your first name", Order = 0)]
    [Required(ErrorMessage = "First name is required")]
    public string FirstName { get; set; } = null!;

    [Display(Name = "Last name", Prompt = "Enter your last name", Order = 1)]
    [Required(ErrorMessage = "Last name is required")]
    public string LastName { get; set; } = null!;

    [Display(Name = "Email address", Prompt = "Enter your Email address", Order = 2)]
    [Required(ErrorMessage = "Email address is required.")]
    [RegularExpression("^[^@\\s]+@[^@\\s]+\\.[^@\\s]{2,}$", ErrorMessage = "Invalid email address.")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Display(Name = "Phone", Prompt = "Enter your phone number", Order = 3)]
    [Required(ErrorMessage = "Phone number is required")]
    [DataType(DataType.PhoneNumber)]
    public string Phone { get; set; } = null!;

    [Display(Name = "Bio", Prompt = "Add a short bio", Order = 4)]
    [DataType(DataType.MultilineText)]
    public string? Biography { get; set; }
}
