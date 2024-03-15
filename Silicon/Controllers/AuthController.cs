using Infrastructure.Entities;
using Infrastructure.Factories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Silicon.ViewModels.Auth;
using System.Security.Claims;

namespace Silicon.Controllers;

public class AuthController(UserService userService, SignInManager<UserEntity> signInManager, UserFactory userFactory) : Controller
{
    private readonly UserService _userService = userService;
    private readonly UserFactory _userFactory = userFactory;
    private readonly SignInManager<UserEntity> _signInManager = signInManager;

    #region Signup
    [Route("/signup")]
    [HttpGet]
    public IActionResult SignUp()
    {
        if (_signInManager.IsSignedIn(User))
            return RedirectToAction("Details", "Account");

        var viewModel = new SignUpViewModel();
        return View(viewModel);
    }

    [Route("/signup")]
    [HttpPost]
    public async Task<IActionResult> SignUp(SignUpViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var result = await _userService.RegisterUserAsync(viewModel.Form);

            if (result.StatusCode == Infrastructure.Models.StatusCode.OK)
                return RedirectToAction("SignIn", "Auth");

            viewModel.ErrorMessage = result.Message;
            return View(viewModel);
        }
        viewModel.ErrorMessage = "Your password must contain at least: One uppercase letter, one lowercase letter, one number and be a minimum of 8 characters long.";
        return View(viewModel);
    }
    #endregion

    #region SignIn
    [Route("/signin")]
    [HttpGet]
    public IActionResult SignIn()
    {
        var viewModel = new SignInViewModel();

        if (_signInManager.IsSignedIn(User))
            return RedirectToAction("Details", "Account");

        return View(viewModel);
    }

    [Route("/signin")]
    [HttpPost]
    public async Task<IActionResult> SignIn(SignInViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var result = await _userService.SignInUserAsync(viewModel.Form);

            if (result.StatusCode == Infrastructure.Models.StatusCode.OK)
                return RedirectToAction("Details", "Account");

            viewModel.ErrorMessage = result.Message;
            return View(viewModel);
        }

        viewModel.ErrorMessage = "Invalid fields: You must enter an email and a password.";
        return View(viewModel);
    }
    #endregion

    #region SignOut
    [Route("/signout")]
    [HttpGet]
    public new async Task<IActionResult> SignOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
    #endregion

    #region External Authentication

    [HttpGet]
    public IActionResult Facebook()
    {
        var authProps = _signInManager.ConfigureExternalAuthenticationProperties("Facebook", Url.Action("FacebookCallBack"));
        return new ChallengeResult("Facebook", authProps);
    }

    [HttpGet]
    public async Task<IActionResult> FacebookCallBack()
    {
        var info = await _signInManager.GetExternalLoginInfoAsync();

        if (info != null)
        {
            var responseResult = _userFactory.PopulateUserEntity(info);

            if (responseResult.StatusCode == Infrastructure.Models.StatusCode.OK)
            {
                var result = await _userService.RegisterOrUpdateExternalAccountAsync((UserEntity)responseResult.ContentResult!);

                if(result.StatusCode != Infrastructure.Models.StatusCode.ERROR)
                {
                    var user = (UserEntity)result.ContentResult!;
                    await _signInManager.SignInAsync(user, isPersistent: false);
                }
                if(HttpContext.User != null)
                    return RedirectToAction("Details", "Account");
            }
        }

        ModelState.AddModelError("InvalidFacebookAuthentication", "Failed to authenticate with Facebook.");
        return RedirectToAction("Signin", "Auth");
    }

    [HttpGet]
    public IActionResult Google()
    {
        var authProps = _signInManager.ConfigureExternalAuthenticationProperties("Google", Url.Action("GoogleCallBack"));
        return new ChallengeResult("Google", authProps);
    }

    [HttpGet]
    public async Task<IActionResult> GoogleCallBack()
    {
        var info = await _signInManager.GetExternalLoginInfoAsync();

        if (info != null)
        {
            var responseResult = _userFactory.PopulateUserEntity(info);

            if (responseResult.StatusCode == Infrastructure.Models.StatusCode.OK)
            {
                var result = await _userService.RegisterOrUpdateExternalAccountAsync((UserEntity)responseResult.ContentResult!);

                if (result.StatusCode != Infrastructure.Models.StatusCode.ERROR)
                {
                    var user = (UserEntity)result.ContentResult!;
                    await _signInManager.SignInAsync(user, isPersistent: false);
                }
                if (HttpContext.User != null)
                    return RedirectToAction("Details", "Account");
            }
        }

        ModelState.AddModelError("InvalidFacebookAuthentication", "Failed to authenticate with Facebook.");
        return RedirectToAction("Signin", "Auth");
    }

    #endregion
}
