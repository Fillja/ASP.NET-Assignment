using Silicon.Models;

namespace Silicon.ViewModels;

public class AccountDetailsViewModel
{
    public string Title { get; set; } = "Account Details";

    public AccountDetailsBasicFormModel BasicForm { get; set; } = new AccountDetailsBasicFormModel
    {
        FirstName = "John",
        LastName = "Doe",
        Email = "john.doe@domain.com",
        ProfileImage = "images/profile-image.svg"
    };
    public AccountDetailsAddressFormModel AddressForm { get; set; } = new AccountDetailsAddressFormModel();
}
