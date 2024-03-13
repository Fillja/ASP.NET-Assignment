using Microsoft.AspNetCore.Mvc;

namespace Silicon.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Home";

        return View();
    }

    [HttpPost]
    public IActionResult Subscribe()
    {
        if(ModelState.IsValid)
        {

        }
        return RedirectToAction("Index");
    }
}
