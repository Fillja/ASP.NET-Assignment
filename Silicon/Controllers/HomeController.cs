using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Silicon.ViewModels.Home;

namespace Silicon.Controllers;

public class HomeController(SubscriberService subscriberService) : Controller
{
    private readonly SubscriberService _subscriberService = subscriberService;

    [HttpGet]
    public IActionResult Index()
    {
        var viewModel = new HomeViewModel();

        if (TempData.ContainsKey("DisplayMessage"))
            viewModel.DisplayMessage = TempData["DisplayMessage"]!.ToString();

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Subscribe(HomeViewModel viewModel)
    {
        if(ModelState.IsValid)
        {
            var createResult = await _subscriberService.ApiCallCreateSubscriberAsync(viewModel.NewsLetter);
            TempData["DisplayMessage"] = createResult.Message;
        }

        return RedirectToAction("Index", "Home", "newsletter");
    }
}
