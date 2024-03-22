using Infrastructure.Contexts;
using Infrastructure.Models.Course;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Silicon_WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CoursesController(CourseService courseService) : ControllerBase
{
    private readonly CourseService _courseService = courseService;

    [HttpPost]
    public async Task<IActionResult> Create(CourseModel model)
    {
        if (ModelState.IsValid)
        {
            var responseResult = await _courseService.CreateCourseAsync(model);

            if(responseResult.StatusCode == Infrastructure.Models.StatusCode.OK)
               return Created();

            if(responseResult.StatusCode == Infrastructure.Models.StatusCode.EXISTS)
                return Conflict();
        }

        return BadRequest();
    }

    //[HttpGet]
    //public IActionResult GetAll()
    //{

    //    return Ok();
    //}
}
