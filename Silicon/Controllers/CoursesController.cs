using Infrastructure.Entities;
using Infrastructure.Models.Course;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Silicon.ViewModels.Courses;

namespace Silicon.Controllers;

[Authorize]
public class CoursesController(UserManager<UserEntity> userManager, CourseService courseService, CategoryRepository categoryRepository) : Controller
{
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly CourseService _courseService = courseService;
    private readonly CategoryRepository _categoryRepository = categoryRepository;

    [Route("/courses")]
    public async Task<IActionResult> Index(string category = "", string searchQuery = "", int pageNumber = 1, int pageSize = 3)
    {
        var viewModel = new CourseViewModel();

        var categoryListResult = await _categoryRepository.GetAllAsync();
        if (categoryListResult.StatusCode == Infrastructure.Models.StatusCode.OK)
            viewModel.CategoryList = (IEnumerable<CategoryEntity>)categoryListResult.ContentResult!;

        var apiResult = await _courseService.ApiCallGetCourseListAsync(category, searchQuery, pageNumber, pageSize);
        if (apiResult.StatusCode == Infrastructure.Models.StatusCode.OK)
        {
            var courseResultModel = (CourseResultModel)apiResult.ContentResult!;
            viewModel.CourseList = courseResultModel.Courses;
            viewModel.Pagination = new PaginationModel
            {
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = courseResultModel.TotalPages,
                TotalItems = courseResultModel.TotalItems
            };
        }

        var user = await _userManager.GetUserAsync(User);
        if (user != null)
        {
            var savedListResult = await _courseService.GetAllSavedCoursesAsync(user);
            if (savedListResult.StatusCode == Infrastructure.Models.StatusCode.OK)
                viewModel.SavedList = (IEnumerable<SavedCoursesEntity>)savedListResult.ContentResult!;
        }

        if (TempData.ContainsKey("DisplayMessage"))
            viewModel.DisplayMessage = TempData["DisplayMessage"]!.ToString();

        return View(viewModel);
    }

    [Route("/singlecourse")]
    public async Task<IActionResult> SingleCourse(string id)
    {
        var courseResult = await _courseService.ApiCallGetOneCourseAsync(id);
        if (courseResult.StatusCode == Infrastructure.Models.StatusCode.OK)
            return View((CourseEntity)courseResult.ContentResult!);

        TempData["DisplayMessage"] = "Something went wrong when loading the course, please try again.";
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Bookmark(string id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user != null)
        {
            var responseResult = await _courseService.AddOrRemoveBookmarkAsync(user, id);
            TempData["DisplayMessage"] = responseResult.Message;
        }
        return RedirectToAction("Index");
    }
}
