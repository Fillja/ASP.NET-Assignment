using Infrastructure.Models.Contact;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Silicon_WebApi.Filters;

namespace Silicon_WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[UseApiKey]
public class ContactController(ContactService contactService) : ControllerBase
{
    private readonly ContactService _contactService = contactService;

    [HttpPost]
    public async Task<IActionResult> Create(ContactModel model)
    {
        if (ModelState.IsValid)
        {
            var createResult = await _contactService.CreateContactAsync(model);

            if (createResult.StatusCode == Infrastructure.Models.StatusCode.OK)
                return Created($"/api/subscribers/{createResult.ContentResult}", createResult.ContentResult);
        }

        return BadRequest();
    }
}
