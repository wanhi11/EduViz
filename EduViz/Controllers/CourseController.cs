using EduViz.Common.Payloads;
using EduViz.Common.Payloads.Response;
using EduViz.Exceptions;
using EduViz.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EduViz.Controllers;
[ApiController]
[Route("api/[controller]")]
public class CourseController:ControllerBase
{
    private readonly CourseService _courseService;

    public CourseController(CourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet("course")]
    public async Task<IActionResult> GetAllCourse()
    {
        var result = await _courseService.GetCoursesWithVipMentorFirst();
        if (!result.Any())
        {
            throw new NotFoundException("there is no course");
        }

        return Ok(ApiResult<GetAllCourseResponse>.Succeed(new GetAllCourseResponse()
        {
            ListCourse = result
        }));
    }
    
    [HttpGet]
    [Route("course/{subjectName}")]
    public async Task<IActionResult> GetAllCourseWithName([FromRoute] string subjectName)
    {
        var result = await _courseService.GetCoursesBySubjectWithVipMentorFirst(subjectName);
        if (!result.Any())
        {
            throw new NotFoundException("there is no course");
        }

        return Ok(ApiResult<GetAllCourseWithName>.Succeed(new GetAllCourseWithName()
        {
            ListCourse = result
        }));
    }
}