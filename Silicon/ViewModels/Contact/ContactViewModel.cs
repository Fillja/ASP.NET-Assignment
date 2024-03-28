using Infrastructure.Models.Contact;

namespace Silicon.ViewModels.Contact;

public class ContactViewModel
{
    public string Title { get; set; } = "Contact";
    public ContactModel ContactModel { get; set; } = new ContactModel();
    public string? DisplayMessage { get; set; }
}
