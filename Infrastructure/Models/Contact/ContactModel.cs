using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models.Contact;

public class ContactModel
{
    [Required(ErrorMessage = "Full name is required.")]
    [Display(Name = "Full Name", Prompt = "Enter your full name", Order = 0)]
    [MinLength(2, ErrorMessage = "You must enter a valid name.")]
    public string FullName { get; set; } = null!;

    [Required(ErrorMessage = "Email is required.")]
    [Display(Name = "Email", Prompt = "Enter your email", Order = 1)]
    [RegularExpression("^[^@\\s]+@[^@\\s]+\\.[^@\\s]{2,}$", ErrorMessage = "You must enter a valid email address.")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Display(Name = "Services", Prompt = "Choose the service you are interested in", Order = 2)]
    public string? Service { get; set; }

    [Required(ErrorMessage = "Message is required.")]
    [Display(Name = "Message", Prompt = "Enter your message here...", Order = 3)]
    [MinLength(2, ErrorMessage = "Please enter a valid message.")]
    public string Message { get; set; } = null!;
}
