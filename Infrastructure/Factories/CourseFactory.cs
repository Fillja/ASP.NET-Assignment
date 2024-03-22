using Infrastructure.Entities;
using Infrastructure.Models.Course;

namespace Infrastructure.Factories;

public class CourseFactory
{
    public CourseEntity PopulateCourseEntity (CourseModel model)
    {
        var courseEntity = new CourseEntity();

        if (model != null)
        {

            courseEntity.Title = model.Title;
            courseEntity.ImageName = model.ImageName;
            courseEntity.Author = model.Author;
            courseEntity.IsBestseller = model.IsBestseller;
            courseEntity.Hours = model.Hours;
            courseEntity.OriginalPrice = model.OriginalPrice;
            courseEntity.DiscountPrice = model.DiscountPrice;
            courseEntity.LikesInProcent = model.LikesInProcent;
            courseEntity.LikesInNumbers = model.LikesInNumbers;
        }

        return courseEntity;
    }
}
