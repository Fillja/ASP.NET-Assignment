using Infrastructure.Entities;
using Infrastructure.Factories;
using Infrastructure.Models;
using Infrastructure.Models.Course;
using Infrastructure.Repositories;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace Infrastructure.Services;

public class CourseService(CourseRepository courseRepository, CourseFactory courseFactory, SavedCoursesRepository savedCoursesRepository)
{
    private readonly CourseRepository _courseRepository = courseRepository;
    private readonly CourseFactory _courseFactory = courseFactory;
    private readonly SavedCoursesRepository _savedCoursesRepository = savedCoursesRepository;

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

    public async Task<ResponseResult> AddOrRemoveBookmarkAsync(UserEntity user, string id)
    {
        try
        {
            Expression<Func<SavedCoursesEntity, bool>> courseExpression = x => x.CourseId == id && x.UserId == user.Id;
            var existResult = await _savedCoursesRepository.ExistsAsync(courseExpression);

            if (existResult.StatusCode == StatusCode.NOT_FOUND)
            {
                var entity = new SavedCoursesEntity { CourseId = id, UserId = user.Id };
                var createResult = await _savedCoursesRepository.CreateAsync(entity);

                if(createResult.StatusCode == StatusCode.OK)
                    return ResponseFactory.Ok(entity, "Course bookmarked successfully.");
            }

            else if (existResult.StatusCode == StatusCode.EXISTS)
            {
                var deleteResult = await _savedCoursesRepository.DeleteAsync(courseExpression);

                if(deleteResult.StatusCode == StatusCode.OK)
                    return ResponseFactory.Ok("Course removed successfully.");
            }

            return ResponseFactory.Error();
        }
        catch (Exception ex) 
        {
            return ResponseFactory.Error(ex.Message);
        }
    }

    public async Task<ResponseResult> GetOneCourseAsync(string id)
    {
        try
        {
            using var http = new HttpClient();
            var response = await http.GetAsync($"https://localhost:7130/api/courses/{id}/?key=MWVhMGJjZjgtZGZhMC00ZjA4LWJiMjctZDQ2NWU0YjQxZWQ5");
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<CourseEntity>(json);

            if(data != null)
                return ResponseFactory.Ok(data);

            return ResponseFactory.NotFound();
        }
        catch(Exception ex) 
        {
            return ResponseFactory.Error(ex.Message);
        }
    }

    public async Task<ResponseResult> GetCourseListAsync()
    {
        try
        {
            using var http = new HttpClient();
            var response = await http.GetAsync("https://localhost:7130/api/courses?key=MWVhMGJjZjgtZGZhMC00ZjA4LWJiMjctZDQ2NWU0YjQxZWQ5");
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<IEnumerable<CourseEntity>>(json);

            if (data != null)
                return ResponseFactory.Ok(data);

            return ResponseFactory.NotFound();
        }
        catch(Exception ex)
        {
            return ResponseFactory.Error(ex.Message);
        }
    }

    public async Task<ResponseResult> GetAllSavedCoursesAsync(UserEntity user)
    {
        try
        {
            var listResult = await _savedCoursesRepository.GetAllAsync();
            if (listResult.StatusCode == StatusCode.OK)
            {
                var list = (IEnumerable<SavedCoursesEntity>)listResult.ContentResult!;
                var savedList = new List<SavedCoursesEntity>();

                foreach (var savedCourseEntity in list)
                {
                    if (savedCourseEntity.UserId == user!.Id)
                        savedList.Add(savedCourseEntity);
                }

                return ResponseFactory.Ok(savedList);
            }

            else if(listResult.StatusCode == StatusCode.NOT_FOUND) 
                return ResponseFactory.NotFound();

            return ResponseFactory.Error();
        }
        catch (Exception ex)
        {
            return ResponseFactory.Error(ex.Message);
        }
    }
}
