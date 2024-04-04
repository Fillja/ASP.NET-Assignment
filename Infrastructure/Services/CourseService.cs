﻿using Infrastructure.Entities;
using Infrastructure.Factories;
using Infrastructure.Models;
using Infrastructure.Models.Course;
using Infrastructure.Repositories;
using System.Linq.Expressions;
using Newtonsoft.Json;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Infrastructure.Services;

public class CourseService(CourseRepository courseRepository, CourseFactory courseFactory, SavedCoursesRepository savedCoursesRepository, DataContext context)
{
    private readonly CourseRepository _courseRepository = courseRepository;
    private readonly CourseFactory _courseFactory = courseFactory;
    private readonly SavedCoursesRepository _savedCoursesRepository = savedCoursesRepository;
    private readonly DataContext _context = context;

    public async Task<ResponseResult> CreateCourseAsync(CourseModel model)
    {
        try
        {
            var existResult = await _courseRepository.ExistsAsync(x => x.Title == model.Title);

            if (existResult.StatusCode == StatusCode.NOT_FOUND)
            {
                var courseEntity = _courseFactory.PopulateCourseEntity(model);

                if (courseEntity != null)
                {
                    var createResult = await _courseRepository.CreateAsync(courseEntity);

                    if (createResult.StatusCode == StatusCode.OK)
                        return ResponseFactory.Ok("Course created successfully.");
                }
            }

            else if (existResult.StatusCode == StatusCode.EXISTS)
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

                if (createResult.StatusCode == StatusCode.OK)
                    return ResponseFactory.Ok(entity, "Course bookmarked successfully.");
            }

            else if (existResult.StatusCode == StatusCode.EXISTS)
            {
                var deleteResult = await _savedCoursesRepository.DeleteAsync(courseExpression);

                if (deleteResult.StatusCode == StatusCode.OK)
                    return ResponseFactory.Ok("Course removed successfully.");
            }

            return ResponseFactory.Error();
        }
        catch (Exception ex)
        {
            return ResponseFactory.Error(ex.Message);
        }
    }

    public async Task<ResponseResult> ApiCallGetOneCourseAsync(string id)
    {
        try
        {
            using var http = new HttpClient();
            var response = await http.GetAsync($"https://localhost:7130/api/courses/{id}/?key=MWVhMGJjZjgtZGZhMC00ZjA4LWJiMjctZDQ2NWU0YjQxZWQ5");
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<CourseEntity>(json);

            if (data != null)
                return ResponseFactory.Ok(data);

            return ResponseFactory.NotFound();
        }
        catch (Exception ex)
        {
            return ResponseFactory.Error(ex.Message);
        }
    }

    public async Task<ResponseResult> ApiCallGetCourseListAsync()
    {
        try
        {
            using var http = new HttpClient();
            var response = await http.GetAsync("https://localhost:7130/api/courses/getall?key=MWVhMGJjZjgtZGZhMC00ZjA4LWJiMjctZDQ2NWU0YjQxZWQ5");
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<IEnumerable<CourseEntity>>(json);

            if (data != null)
                return ResponseFactory.Ok(data);

            return ResponseFactory.NotFound();
        }
        catch (Exception ex)
        {
            return ResponseFactory.Error(ex.Message);
        }
    }

    public async Task<ResponseResult> ApiCallGetCourseListAsync(string category, string searchQuery, int pageNumber, int pageSize)
    {
        try
        {
            using var http = new HttpClient();

            var queryParameters = new Dictionary<string, string>
            {
                { "key", "MWVhMGJjZjgtZGZhMC00ZjA4LWJiMjctZDQ2NWU0YjQxZWQ5" },
                { "category", Uri.EscapeDataString(category) },
                { "searchQuery", Uri.EscapeDataString(searchQuery) },
                { "pageNumber", pageNumber.ToString() },
                { "pageSize", pageSize.ToString() }
            };
            var urlBuilder = new StringBuilder("https://localhost:7130/api/courses/getallwithfilters?");

            foreach (var param in queryParameters)
            {
                urlBuilder.Append($"{param.Key}={param.Value}&");
            }
            var url = urlBuilder.ToString().TrimEnd('&');

            var response = await http.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<CourseResultModel>(json);

            if (data!.Courses != null)
                return ResponseFactory.Ok(data);

            return ResponseFactory.NotFound();
        }
        catch (Exception ex)
        {
            return ResponseFactory.Error(ex.Message);
        }
    }

    public async Task<ResponseResult> GetAllFilteredCoursesAsync(string category, string searchQuery, int pageNumber, int pageSize)
    {
        try
        {
            var query = _context.Courses.Include(c => c.CourseCategories!).ThenInclude(cc => cc.Category).AsQueryable();

            if (!string.IsNullOrEmpty(category) && category != "all")
                query = query.Where(c => c.CourseCategories!.Any(cc => cc.Category.CategoryName == category));

            if (!string.IsNullOrEmpty(searchQuery))
                query = query.Where(c => c.Title.Contains(searchQuery) || c.Author!.Contains(searchQuery));

            query = query.OrderByDescending(x => x.Title);

            var response = new CourseResultModel
            {
                TotalItems = await query.CountAsync()
            };
            response.TotalPages = (int)Math.Ceiling(response.TotalItems / (double)pageSize);
            response.Courses = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return ResponseFactory.Ok(response);
        }
        catch (Exception ex)
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

            else if (listResult.StatusCode == StatusCode.NOT_FOUND)
                return ResponseFactory.NotFound();

            return ResponseFactory.Error();
        }
        catch (Exception ex)
        {
            return ResponseFactory.Error(ex.Message);
        }
    }

    public async Task<ResponseResult> DeleteAllSavedCourses(UserEntity user)
    {
        try
        {
            var savedListResult = await GetAllSavedCoursesAsync(user);
            if (savedListResult.StatusCode == StatusCode.OK)
            {
                var savedList = (IEnumerable<SavedCoursesEntity>)savedListResult.ContentResult!;
                foreach (var savedCourse in savedList)
                {
                    var deleteResult = await _savedCoursesRepository.DeleteAsync(savedCourse);
                    if (deleteResult.StatusCode == StatusCode.OK)
                        continue;

                    return ResponseFactory.Error("Something went wrong with deleting the courses.");
                }

                return ResponseFactory.Ok("Successfully deleted all saved courses.");
            }

            else if (savedListResult.StatusCode == StatusCode.NOT_FOUND)
                return ResponseFactory.NotFound("Could not find any courses.");

            return ResponseFactory.Error("Something went wrong with deleting the courses.");
        }
        catch (Exception ex)
        {
            return ResponseFactory.Error(ex.Message);
        }
    }
}
