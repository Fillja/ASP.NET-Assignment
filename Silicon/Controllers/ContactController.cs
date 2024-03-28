using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Silicon.ViewModels.Contact;

namespace Silicon.Controllers;

public class ContactController(ContactService contactService) : Controller
{
    private readonly ContactService _contactService = contactService;

    [HttpGet]
    [Route("/contact")]
    public IActionResult Index()
    {
        var viewModel = new ContactViewModel();

        if (TempData.ContainsKey("DisplayMessage"))
            viewModel.DisplayMessage = TempData["DisplayMessage"]!.ToString();

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> SendRequest(ContactViewModel viewModel)
    {
        TempData["DisplayMessage"] = "You must fill out all the neccessary fields.";

        if (ModelState.IsValid)
        {
            var createResult = await _contactService.ApiCallCreateContactRequestAsync(viewModel.ContactModel);
            TempData["DisplayMessage"] = createResult.Message;

        }

        return RedirectToAction("Index");
    }
}
