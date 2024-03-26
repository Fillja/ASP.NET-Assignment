using Infrastructure.Entities;
using Infrastructure.Models.Course;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
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

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var responseResult = await _courseRepository.GetOneAsync(x => x.Id == id);

        if (responseResult.StatusCode == Infrastructure.Models.StatusCode.OK)
            return Ok((CourseEntity)responseResult.ContentResult!);

        else if (responseResult.StatusCode == Infrastructure.Models.StatusCode.NOT_FOUND)
            return NotFound();

        return BadRequest();
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> Update(CourseEntity courseEntity)
    {
        if (ModelState.IsValid) 
        {
            var updateResult = await _courseRepository.UpdateAsync(courseEntity, (x => x.Id == courseEntity.Id));

            if (updateResult.StatusCode == Infrastructure.Models.StatusCode.OK)
                return Ok((CourseEntity)updateResult.ContentResult!);

            else if (updateResult.StatusCode == Infrastructure.Models.StatusCode.NOT_FOUND)
                return NotFound();
        }

        return BadRequest();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var deleteResult = await _courseRepository.DeleteAsync(x => x.Id == id);

        if (deleteResult.StatusCode == Infrastructure.Models.StatusCode.OK)
            return Ok();

        else if (deleteResult.StatusCode == Infrastructure.Models.StatusCode.NOT_FOUND)
            return NotFound();

        return BadRequest();
    }
}
