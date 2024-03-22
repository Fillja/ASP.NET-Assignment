using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

[PrimaryKey("UserId", "CourseId")]
public class SavedCoursesEntity
{
    [Key]
    public string UserId { get; set; } = null!;

    [ForeignKey("UserId")]
    public UserEntity User { get; set; } = null!;

    [Key]
    public string CourseId { get; set; } = null!;

    [ForeignKey("CourseId")]
    public CourseEntity Course { get; set; } = null!;
}
