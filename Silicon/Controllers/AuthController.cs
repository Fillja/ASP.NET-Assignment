using Microsoft.AspNetCore.Mvc;
using Silicon.ViewModels.Auth;

namespace Silicon.Controllers;

public class AuthController : Controller
{
    [Route("/signup")]
    [HttpGet]
    public IActionResult SignUp()
    {
        var viewModel = new SignUpViewModel();
        return View(viewModel);
    }

    [Route("/signup")]
    [HttpPost]
    public IActionResult SignUp(SignUpViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            viewModel.ErrorMessage = "Your password must contain at least: One uppercase letter, one lowercase letter, one number and be a minimum of 8 characters long.";
            return View(viewModel);
        }
        return RedirectToAction("SignIn", "Auth");
    }

    [Route("/signin")]
    [HttpGet]
    public IActionResult SignIn()
    {
        var viewModel = new SignInViewModel();
        return View(viewModel);
    }

    [Route("/signin")]
    [HttpPost]
    public IActionResult SignIn(SignInViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        viewModel.ErrorMessage = "Incorrect email or password.";
        return View(viewModel);

        //return RedirectToAction("Details", "Account");
    }
}
