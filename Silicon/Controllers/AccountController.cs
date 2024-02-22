using Microsoft.AspNetCore.Mvc;
using Silicon.ViewModels;

namespace Silicon.Controllers;

public class AccountController : Controller
{

    [Route("/Details")]
    [HttpGet]
    public IActionResult Details()
    {
        var viewModel = new AccountDetailsViewModel();

        //viewmodel.BasicForm = _accountService.GetBasicForm()
        //viewmodel.AddressForm = _accountService.GetAddressForm()
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
