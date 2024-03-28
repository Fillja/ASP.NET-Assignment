using Infrastructure.Models.Home;

namespace Silicon.ViewModels.Home;

public class HomeViewModel
{
    public string Title { get; set; } = "Home";
    public NewsLetterModel NewsLetter { get; set; } = new NewsLetterModel();
    public string? DisplayMessage { get; set; }
}
