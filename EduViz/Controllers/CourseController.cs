using Azure.Core;
using EduViz.Common.Payloads;
using EduViz.Common.Payloads.Request;
using EduViz.Common.Payloads.Response;
using EduViz.Enums;
using EduViz.Exceptions;
using EduViz.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EduViz.Controllers;
[ApiController]
[Route("api/[controller]")]
public class CourseController:ControllerBase
{
    private readonly CourseService _courseService;
    private readonly SubjectService _subjectService;
    private readonly UserService _userService;
    private readonly MentorDetailService _mentorService;
    private readonly CloudinaryService _cloudinaryService;

    public CourseController(CourseService courseService, SubjectService subjectService,
        UserService userService,MentorDetailService mentorService,CloudinaryService cloudinaryService)
    {
        _courseService = courseService;
        _subjectService = subjectService;
        _userService = userService;
        _mentorService = mentorService;
        _cloudinaryService = cloudinaryService;
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

    [HttpPost]
    [Route("create-course")]
    [Authorize(Roles = "Mentor")]
    public async Task<IActionResult> CreateNewCourse([FromForm] CreateCourseRequest req)
    {
        Request.Headers.TryGetValue("Authorization", out var token);
        token = token.ToString().Split()[1];
        var currentUser = _userService.GetUserInToken(token);
        var mentor = _mentorService.GetByMentorId(currentUser.UserId);
        var course = req.ToCourseModel();
        var subject = _subjectService.GetSubjectByName(req.SubjectName);
        course.CourseId = subject.SubjectId;
        course.MentorId = mentor.MentorDetailsId;
        
        if (req.Picture != null)
        {
            var uploadResult = await _cloudinaryService.UploadImageAsync(req.Picture);

            if (uploadResult.Error != null)
            {
                throw new BadRequestException("Failed to upload image.");
            }

            course.Picture = uploadResult.SecureUrl.ToString(); // Lưu đường dẫn của hình ảnh
        }
        var result = await _courseService.CreateCourse(course);
        if (course is null)
        {
            throw new BadRequestException("something went wrong when create course");
        }

        return Ok(ApiResult<CreateCourseResponse>.Succeed(new CreateCourseResponse()
        {
            createdCourse = course
        }));
    }

    [HttpGet]
    [Route("get-week-schedule")]
    public IActionResult GetWeekSchedule()
    {
        return Ok(ApiResult<GetWeekScheduleResponse>.Succeed(new GetWeekScheduleResponse()
        {
            WeekSchedule = new List<string>()
            {
                Schedule.SatSun.ToString(),
                Schedule.MonWedFri.ToString(),
                Schedule.TueThuSat.ToString(),
            }
        }));
    }
    
}