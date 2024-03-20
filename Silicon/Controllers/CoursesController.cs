using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Silicon.Controllers;

[Authorize]
public class CoursesController : Controller
{
    [Route("/courses")]
    public IActionResult Index()
    {
        ViewData["Title"] = "Courses";
        return View();
    }

    [Route("/singlecourse")]
    public IActionResult SingleCourse()
    {
        return View();
    }

    public IActionResult Bookmark()
    {
        return View();
    }
}
