using Infrastructure.Entities;
using Infrastructure.Factories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Silicon.ViewModels.Account;

namespace Silicon.Controllers;

[Authorize]
public class AccountController(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager, UserFactory userFactory, AddressFactory addressFactory) : Controller
{
    private readonly SignInManager<UserEntity> _signInManager = signInManager;
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly UserFactory _userFactory = userFactory;
    private readonly AddressFactory _addressFactory = addressFactory;

    [Route("/Details")]
    [HttpGet]
    public async Task<IActionResult> Details()
    {
        var viewModel = new AccountDetailsViewModel();
        var userEntity = await _userManager.GetUserAsync(User);

        if(userEntity != null) 
        {
            viewModel.BasicForm = _userFactory.PopulateBasicForm(userEntity);
            viewModel.AddressForm = _addressFactory.PopulateAddressForm(userEntity);
        }

        return View(viewModel);
    }

    [Route("/Details")]
    [HttpPost]
    public IActionResult Details(AccountDetailsViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }
        return View(viewModel);
    }

    [HttpPost]
    public IActionResult BasicInfo(AccountDetailsViewModel viewModel) 
    {
        //_accountService.SaveBasicForm(viewModel.BasicForm);
        return RedirectToAction(nameof(Details));
    }

    [HttpPost]
    public IActionResult AddressInfo(AccountDetailsViewModel viewModel)
    {
        //_accountService.SaveAddressForm(viewModel.AddressForm);
        return RedirectToAction(nameof(Details));
    }
}
