using Infrastructure.Entities;

namespace Infrastructure.Models.Course;

public class CourseResultModel
{
    public int TotalPages { get; set; }
    public int TotalItems { get; set; }
    public IEnumerable<CourseEntity>? Courses { get; set; }
}
