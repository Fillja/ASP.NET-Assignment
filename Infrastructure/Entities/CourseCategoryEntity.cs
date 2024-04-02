using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

[PrimaryKey("CategoryId", "CourseId")]
public class CourseCategoryEntity
{
    [Key]
    public string CategoryId { get; set; } = null!;

    [ForeignKey("CategoryId")]
    public CategoryEntity Category { get; set; } = new CategoryEntity();

    [Key]
    public string CourseId { get; set; } = null!;

    [ForeignKey("CourseId")]
    public CourseEntity Course { get; set; } = new CourseEntity();
}
