using Silicon.Models;

namespace Silicon.ViewModels;

public class SignUpViewModel
{
    public string Title { get; set; } = "Sign up";

    public SignUpFormModel Form { get; set; } = new SignUpFormModel();

    public string? ErrorMessage { get; set; }
}
