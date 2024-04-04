using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models.Home;

public class NewsLetterModel
{
    [Display(Name = "Email address", Prompt = "Your Email", Order = 0)]
    [Required(ErrorMessage = "Email address is required.")]
    [RegularExpression("^[^@\\s]+@[^@\\s]+\\.[^@\\s]{2,}$", ErrorMessage = "Invalid email address.")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Display(Name = "Daily Newsletter", Order  = 1)]
    public bool DailyNewsletter { get; set; }

    [Display(Name = "Advertising Updates", Order = 2)]
    public bool AdvertisingUpdates { get; set; }

    [Display(Name = "Week in Review", Order = 3)]
    public bool WeekInReview { get; set; }

    [Display(Name = "Event Updates", Order = 4)]
    public bool EventUpdates { get; set; }

    [Display(Name = "Startups Weekly", Order = 5)]
    public bool StartupsWeekly { get; set; }

    [Display(Name = "Podcasts", Order = 6)]
    public bool Podcasts { get; set; }

}
