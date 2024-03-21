using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Silicon_WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CoursesController : ControllerBase
{
    [HttpPost]
    public IActionResult Create()
    {
        return Ok();
    }

    [HttpGet]
    public IActionResult GetAll()
    {

        return Ok();
    }
}
