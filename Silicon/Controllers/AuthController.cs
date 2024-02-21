using Microsoft.AspNetCore.Mvc;

namespace Silicon.Controllers;

public class AuthController : Controller
{
    public IActionResult SignUp()
    {
        ViewData["Title"] = "Sign up";

        return View();
    }

    public IActionResult SignIn()
    {
        return View();
    }
}
