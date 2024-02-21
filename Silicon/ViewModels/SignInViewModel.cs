using Silicon.Models;

namespace Silicon.ViewModels;

public class SignInViewModel
{
    public string Title { get; set; } = "Sign in";

    public SignInFormModel Form { get; set; } = new SignInFormModel();

    public string? ErrorMessage { get; set; }
}
