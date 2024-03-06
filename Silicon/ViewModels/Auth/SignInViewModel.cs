using Infrastructure.Models.Auth;

namespace Silicon.ViewModels.Auth;

public class SignInViewModel
{
    public string Title { get; set; } = "Sign in";

    public SignInFormModel Form { get; set; } = new SignInFormModel();

    public string? ErrorMessage { get; set; }
}
