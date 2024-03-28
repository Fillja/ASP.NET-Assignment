using Infrastructure.Entities;
using Infrastructure.Models.Account;

namespace Silicon.ViewModels.Account;

public class AccountSavedCoursesViewModel
{
    public string Title { get; set; } = "Saved Courses";
    public string? DisplayMessage { get; set; }
    public AccountDetailsBasicFormModel BasicForm { get; set; } = new AccountDetailsBasicFormModel();
    public IEnumerable<CourseEntity> AllCourses { get; set; } = [];
    public IEnumerable<SavedCoursesEntity> SavedCourses { get; set; } = [];
}
