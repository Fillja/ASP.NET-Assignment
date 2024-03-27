using Infrastructure.Entities;

namespace Silicon.ViewModels.Courses;

public class CourseViewModel
{
    public string Title { get; set; } = "Courses";
    public IEnumerable<CourseEntity>? List { get; set; }
    public IEnumerable<SavedCoursesEntity>? SavedList { get; set; }
    public string? DisplayMessage { get; set; }
}
