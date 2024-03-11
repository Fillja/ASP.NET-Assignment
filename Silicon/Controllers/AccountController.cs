using Infrastructure.Entities;
using Infrastructure.Factories;
using Infrastructure.Models.Account;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Silicon.ViewModels.Account;

namespace Silicon.Controllers;

[Authorize]
public class AccountController(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager, UserFactory userFactory, AddressFactory addressFactory, AddressService addressService, UserRepository userRepository, AddressRepository addressRepository) : Controller
{
    private readonly SignInManager<UserEntity> _signInManager = signInManager;
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly AddressService _addressService = addressService;
    private readonly UserRepository _userRepository = userRepository;
    private readonly AddressRepository _addressRepository = addressRepository;
    private readonly UserFactory _userFactory = userFactory;
    private readonly AddressFactory _addressFactory = addressFactory;

    [Route("/Details")]
    [HttpGet]
    public async Task<IActionResult> Details()
    {
        var viewModel = new AccountDetailsViewModel();
        var userEntity = await _userManager.GetUserAsync(User);

        if (userEntity != null)
        {
            viewModel.BasicForm = _userFactory.PopulateBasicForm(userEntity);
            viewModel.AddressForm = _addressFactory.PopulateAddressForm(userEntity);
        }

        return View(viewModel);
    }

    //[Route("/Details")]
    //[HttpPost]
    //public async Task<IActionResult> Details(AccountDetailsViewModel viewModel)
    //{
    //    var userEntity = await _userManager.GetUserAsync(User);

    //    if (ModelState["BasicForm"]!.ValidationState == ModelValidationState.Valid)
    //    {
    //        if (userEntity != null)
    //        {
    //            var responseResult = _userFactory.PopulateUserEntity(viewModel.BasicForm, userEntity);
    //            var updateResult = await _userManager.UpdateAsync((UserEntity)responseResult.ContentResult!);

    //            if (updateResult.Succeeded)
    //            {
    //                userEntity = await _userManager.GetUserAsync(User);
    //                viewModel.BasicForm = _userFactory.PopulateBasicForm(userEntity!);
    //            }
    //        }
    //    }
    //    if (ModelState["AddressForm"]!.ValidationState == ModelValidationState.Valid)
    //    {
    //        var responseResult = await _addressService.UpdateUserWithAddress(userEntity!, viewModel.AddressForm);
    //        if (responseResult.StatusCode == Infrastructure.Models.StatusCode.OK)
    //        {
    //            userEntity = await _userManager.GetUserAsync(User);
    //            viewModel.AddressForm = _addressFactory.PopulateAddressForm(userEntity!);
    //        }
    //    }

    //    return View(viewModel);
    //}

    [HttpPost]
    public async Task<IActionResult> UpdateBasicForm(AccountDetailsBasicFormModel model)
    {
        var userEntity = await _userManager.GetUserAsync(User);

        if (ModelState.IsValid)
        {
            if (userEntity != null)
            {
                var responseResult = _userFactory.PopulateUserEntity(model, userEntity);
                await _userManager.UpdateAsync((UserEntity)responseResult.ContentResult!);
            }
        }

        return RedirectToAction("Details");
    }


    [HttpPost]
    public async Task<IActionResult> UpdateAddressForm(AccountDetailsAddressFormModel model)
    {
        var userEntity = await _userManager.GetUserAsync(User);

        if (ModelState.IsValid)
        {
            await _addressService.UpdateUserWithAddress(userEntity!, model);
        }

        return RedirectToAction("Details");
    }
}
