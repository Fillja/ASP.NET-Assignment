using Infrastructure.Models.Account;

namespace Silicon.ViewModels.Account;

public class AccountDetailsViewModel
{
    public string Title { get; set; } = "Account Details";

    public string? BasicDisplayMessage { get; set; }

    public string? AddressDisplayMessage { get; set; }

    public AccountDetailsBasicFormModel BasicForm { get; set; } = new AccountDetailsBasicFormModel();
    public AccountDetailsAddressFormModel AddressForm { get; set; } = new AccountDetailsAddressFormModel();
}
