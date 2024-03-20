using Infrastructure.Entities;
using Infrastructure.Factories;
using Infrastructure.Models;
using Infrastructure.Models.Account;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Silicon.ViewModels.Account;

namespace Silicon.Controllers;

[Authorize]
public class AccountController(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager, UserFactory userFactory, AddressFactory addressFactory, AddressService addressService, UserService userService) : Controller
{
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly SignInManager<UserEntity> _signInManager = signInManager;
    private readonly AddressService _addressService = addressService;
    private readonly UserService _userService = userService;
    private readonly UserFactory _userFactory = userFactory;
    private readonly AddressFactory _addressFactory = addressFactory;

    #region Account Details

    [Route("/Details")]
    [HttpGet]
    public async Task<IActionResult> Details()
    {
        var viewModel = new AccountDetailsViewModel();
        var userEntity = await _userManager.GetUserAsync(User);

        if (userEntity != null)
        {
            viewModel.BasicForm = _userFactory.PopulateBasicForm(userEntity);
            if (TempData.ContainsKey("BasicDisplayMessage"))
                viewModel.BasicDisplayMessage = TempData["BasicDisplayMessage"]!.ToString();

            viewModel.AddressForm = await _addressFactory.PopulateAddressForm(userEntity);
            if (TempData.ContainsKey("AddressDisplayMessage"))
                viewModel.AddressDisplayMessage = TempData["AddressDisplayMessage"]!.ToString();
        }

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateBasicForm([Bind(Prefix = "BasicForm")] AccountDetailsBasicFormModel model)
    {
        TempData["BasicDisplayMessage"] = "You must fill out all the necessary fields.";

        var userEntity = await _userManager.GetUserAsync(User);
        var externalUser = await _signInManager.GetExternalLoginInfoAsync();

        if(externalUser != null)
        {
            var claims = HttpContext.User.Identities.FirstOrDefault();
            var responseResult = await _userService.UpdateExternalBasicInfoAsync(externalUser, model, claims!);
            TempData["BasicDisplayMessage"] = responseResult.Message;
        }

        else if (TryValidateModel(model))
        {
            if (userEntity != null)
            {
                var responseResult = await _userService.UpdateBasicInfoAsync(userEntity, model);
                TempData["BasicDisplayMessage"] = responseResult.Message;
            }
        }

        return RedirectToAction("Details");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateAddressForm([Bind(Prefix = "AddressForm")] AccountDetailsAddressFormModel model)
    {
        var userEntity = await _userManager.GetUserAsync(User);
        TempData["AddressDisplayMessage"] = "You must fill out all the necessary fields.";

        if (TryValidateModel(model))
        {
            if (userEntity != null)
            {
                var responseResult = await _addressService.UpdateUserWithAddress(userEntity!, model);
                TempData["AddressDisplayMessage"] = responseResult.Message;
            }
        }

        return RedirectToAction("Details");
    }

    #endregion

    #region Account Security 

    [HttpGet]
    [Route("/security")]
    public async Task<IActionResult> Security()
    {
        var viewModel = new AccountSecurityViewModel();
        var userEntity = await _userManager.GetUserAsync(User);

        if (userEntity != null)
        {
            viewModel.BasicForm = _userFactory.PopulateBasicForm(userEntity);
            if (TempData.ContainsKey("DisplayMessage"))
                viewModel.DisplayMessage = TempData["DisplayMessage"]!.ToString();
        }

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword([Bind(Prefix = "PasswordModel")]AccountSecurityPasswordModel model)
    {
        TempData["DisplayMessage"] = "One of the fields were invalid.";
        var userEntity = await _userManager.GetUserAsync(User);

        if (TryValidateModel(model))
        {
            if (userEntity != null)
            {
                var passwordResult = await _userManager.ChangePasswordAsync(userEntity, model.OldPassword, model.NewPassword);

                if (passwordResult.Succeeded)
                {
                    TempData["DisplayMessage"] = "Password updated successfully.";
                }
            }
        }

        return RedirectToAction("security");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteAccount([Bind(Prefix = "DeleteModel")]AccountSecurityDeleteModel model)
    {
        TempData["DisplayMessage"] = "Something went wrong with the removal of your account, please try again.";
        var userEntity = await _userManager.GetUserAsync(User);

        if(TryValidateModel(model))
        {
            if (userEntity != null)
            {
                var deleteResult = await _userManager.DeleteAsync(userEntity);

                if(deleteResult.Succeeded)
                {
                    return RedirectToAction("SignOut", "Auth");
                }
            }
        }

        return RedirectToAction("security");
    }

    #endregion

    #region Account Saved Courses

    public async Task<IActionResult> SavedCourses()
    {
        var viewModel = new AccountDetailsViewModel();
        var userEntity = await _userManager.GetUserAsync(User);

        if (userEntity != null)
        {
            viewModel.BasicForm = _userFactory.PopulateBasicForm(userEntity);
            if (TempData.ContainsKey("BasicDisplayMessage"))
                viewModel.BasicDisplayMessage = TempData["BasicDisplayMessage"]!.ToString();

            viewModel.AddressForm = await _addressFactory.PopulateAddressForm(userEntity);
            if (TempData.ContainsKey("AddressDisplayMessage"))
                viewModel.AddressDisplayMessage = TempData["AddressDisplayMessage"]!.ToString();
        }

        return View(viewModel);
    }

    #endregion
}
