using Infrastructure.Entities;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Silicon.ViewModels.Courses;

namespace Silicon.Controllers;

[Authorize]
public class CoursesController(UserManager<UserEntity> userManager, CourseService courseService, SavedCoursesRepository savedCoursesRepository) : Controller
{
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly CourseService _courseService = courseService;
    private readonly SavedCoursesRepository _savedCoursesRepository = savedCoursesRepository;

    [Route("/courses")]
    public async Task<IActionResult> Index()
    {
        var viewModel = new CourseViewModel();

        var courseListResult = await _courseService.ApiCallGetCourseListAsync();
        if(courseListResult.StatusCode == Infrastructure.Models.StatusCode.OK)
            viewModel.List = (IEnumerable<CourseEntity>)courseListResult.ContentResult!;

        var user = await _userManager.GetUserAsync(User);
        if(user != null)
        {
            var savedListResult = await _courseService.GetAllSavedCoursesAsync(user);
            if(savedListResult.StatusCode == Infrastructure.Models.StatusCode.OK)
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
        if(courseResult.StatusCode == Infrastructure.Models.StatusCode.OK)
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
