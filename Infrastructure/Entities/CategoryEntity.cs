using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities;

public class CategoryEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<CourseCategoryEntity>? CourseCategories { get; set; }
}
