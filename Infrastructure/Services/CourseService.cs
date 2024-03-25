using Infrastructure.Factories;
using Infrastructure.Models;
using Infrastructure.Models.Course;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class CourseService(CourseRepository courseRepository, CourseFactory courseFactory)
{
    private readonly CourseRepository _courseRepository = courseRepository;
    private readonly CourseFactory _courseFactory = courseFactory;

    public async Task<ResponseResult> CreateCourseAsync(CourseModel model)
    {
        try
        {
            var existResult = await _courseRepository.ExistsAsync(x => x.Title == model.Title);

            if(existResult.StatusCode == StatusCode.NOT_FOUND)
            {
                var courseEntity = _courseFactory.PopulateCourseEntity(model);

                if(courseEntity != null)
                {
                    var createResult = await _courseRepository.CreateAsync(courseEntity);

                    if(createResult.StatusCode == StatusCode.OK)
                        return ResponseFactory.Ok("Course created successfully.");
                }
            }

            else if(existResult.StatusCode == StatusCode.EXISTS)
            {
                return ResponseFactory.Exists("A course with this name already exists.");
            }

            return ResponseFactory.Error("Something went wrong with creating the course.");
        }
        catch (Exception ex)
        {
            return ResponseFactory.Error(ex.Message);
        }
    }
}
