using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Models.Course;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Silicon_WebApi.Filters;

namespace Silicon_WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[UseApiKey]
public class CoursesController(CourseService courseService, CourseRepository courseRepository) : ControllerBase
{
    private readonly CourseService _courseService = courseService;
    private readonly CourseRepository _courseRepository = courseRepository;

    [HttpPost]
    public async Task<IActionResult> Create(CourseModel model)
    {
        if (ModelState.IsValid)
        {
            var responseResult = await _courseService.CreateCourseAsync(model);

            if (responseResult.StatusCode == Infrastructure.Models.StatusCode.OK)
                return Created($"/api/courses/{responseResult.ContentResult}", responseResult.ContentResult);

            if(responseResult.StatusCode == Infrastructure.Models.StatusCode.EXISTS)
                return Conflict();
        }

        return BadRequest();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var responseResult = await _courseRepository.GetAllAsync();

        if (responseResult.StatusCode == Infrastructure.Models.StatusCode.OK)
            return Ok((IEnumerable<CourseEntity>)responseResult.ContentResult!);

        else if (responseResult.StatusCode == Infrastructure.Models.StatusCode.NOT_FOUND)
            return NotFound();

        return BadRequest();
    }

    [HttpGet("{title}")]
    public async Task<IActionResult> Get(string title)
    {
        var responseResult = await _courseRepository.GetOneAsync(x => x.Title == title);

        if (responseResult.StatusCode == Infrastructure.Models.StatusCode.OK)
            return Ok((CourseEntity)responseResult.ContentResult!);

        else if (responseResult.StatusCode == Infrastructure.Models.StatusCode.NOT_FOUND)
            return NotFound();

        return BadRequest();
    }
}
