using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Silicon.Controllers;

[Authorize]
public class CoursesController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Courses";
        return View();
    }

    public IActionResult SingleCourse()
    {
        return View();
    }

    public IActionResult Bookmark()
    {
        return View();
    }
}
