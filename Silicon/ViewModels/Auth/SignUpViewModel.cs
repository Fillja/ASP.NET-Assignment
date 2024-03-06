using Infrastructure.Models.Auth;

namespace Silicon.ViewModels.Auth;

public class SignUpViewModel
{
    public string Title { get; set; } = "Sign up";

    public SignUpFormModel Form { get; set; } = new SignUpFormModel();

    public string? ErrorMessage { get; set; }
}
