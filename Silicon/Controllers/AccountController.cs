using Microsoft.AspNetCore.Mvc;

namespace Silicon.Controllers;

public class AccountController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
