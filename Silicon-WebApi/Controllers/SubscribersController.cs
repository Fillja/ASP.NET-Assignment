using Infrastructure.Models.Home;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Silicon_WebApi.Filters;

namespace Silicon_WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[UseApiKey]
public class SubscribersController(SubscriberService subscriberService, SubscriberRepository subscriberRepository) : ControllerBase
{
    private readonly SubscriberRepository _subscriberRepository = subscriberRepository;
    private readonly SubscriberService _subscriberService = subscriberService;

    [HttpPost]
    public async Task<IActionResult> Create(NewsLetterModel model)
    {
        if (ModelState.IsValid)
        {
            var createResult = await _subscriberService.CreateSubscriberAsync(model);

            if (createResult.StatusCode == Infrastructure.Models.StatusCode.OK)
                return Created($"/api/subscribers/{createResult.ContentResult}", createResult.ContentResult);

            else if (createResult.StatusCode == Infrastructure.Models.StatusCode.EXISTS)
                return Conflict();
        }

        return BadRequest();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleteResult = await _subscriberRepository.DeleteAsync(x => x.Id == id);

        if (deleteResult.StatusCode == Infrastructure.Models.StatusCode.OK)
            return Ok();

        else if (deleteResult.StatusCode == Infrastructure.Models.StatusCode.NOT_FOUND)
            return NotFound();

        return BadRequest();
    }
}
