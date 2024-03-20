using Infrastructure.Models.Account;

namespace Silicon.ViewModels.Account;

public class AccountSecurityViewModel
{
    public string Title { get; set; } = "Account Security";
    public string? DisplayMessage { get; set; }
    public AccountSecurityDeleteModel DeleteModel { get; set; } = new AccountSecurityDeleteModel();
    public AccountSecurityPasswordModel PasswordModel { get; set; } = new AccountSecurityPasswordModel();
    public AccountDetailsBasicFormModel BasicForm { get; set; } = new AccountDetailsBasicFormModel();
}
