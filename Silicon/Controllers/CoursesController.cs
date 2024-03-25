using Infrastructure.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Silicon.Controllers;

[Authorize]
public class CoursesController : Controller
{
    [Route("/courses")]
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Courses";

        using var http = new HttpClient();
        var response = await http.GetAsync("https://localhost:7130/api/courses?key=MWVhMGJjZjgtZGZhMC00ZjA4LWJiMjctZDQ2NWU0YjQxZWQ5");
        var json = await response.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<IEnumerable<CourseEntity>>(json);

        return View(data);
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
