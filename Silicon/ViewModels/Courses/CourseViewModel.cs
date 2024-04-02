using Infrastructure.Entities;
using Infrastructure.Models.Course;

namespace Silicon.ViewModels.Courses;

public class CourseViewModel
{
    public string Title { get; set; } = "Courses";
    public IEnumerable<CourseEntity>? CourseList { get; set; }
    public IEnumerable<SavedCoursesEntity>? SavedList { get; set; }
    public IEnumerable<CategoryEntity>? CategoryList { get; set; }
    public PaginationModel? Pagination { get; set; }
    public string? DisplayMessage { get; set; }
}
