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
[Route("api/course")]
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

        var listResult = new List<CourseResponse>();
        foreach (var course in result)
        {
            var subject = _subjectService.GetSubjectById(course.SubjectId);
            var mentor = _mentorService.GetById(course.MentorId);
            var user =await _userService.GetUserById(mentor.UserId);
            listResult.Add(new CourseResponse()
            {
                Schedule = course.Schedule.ToString(),
                CourseName = course.CourseName,
                StartDate = course.StartDate,
                Duration = course.Duration,
                Price = course.Price,
                Picture = course.Picture,
                SubjectName = subject.SubjectName,
                MentorName = user.UserName,
                CourseId = course.CourseId
            });
        }
        return Ok(ApiResult<GetAllCourseResponse>.Succeed(new GetAllCourseResponse()
        {
            ListCourse = listResult
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
        var listResult = new List<CourseResponse>();
        foreach (var course in result)
        {
            var subject = _subjectService.GetSubjectById(course.SubjectId);
            var mentor = _mentorService.GetById(course.MentorId);
            var user =await _userService.GetUserById(mentor.UserId);
            listResult.Add(new CourseResponse()
            {
                Schedule = course.Schedule.ToString(),
                CourseName = course.CourseName,
                StartDate = course.StartDate,
                Duration = course.Duration,
                Price = course.Price,
                Picture = course.Picture,
                SubjectName = subject.SubjectName,
                MentorName = user.UserName,
                CourseId = course.CourseId
            });
        }

        return Ok(ApiResult<GetAllCourseWithName>.Succeed(new GetAllCourseWithName()
        {
            ListCourse = listResult
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

    [HttpGet]
    [Route("detail/{courseId:guid}")]
    public async Task<IActionResult> GetCourseDetail([FromRoute] Guid courseId )
    {
        var course = _courseService.GetCourseById(courseId);
        var subject = _subjectService.GetSubjectById(course.SubjectId);
        var mentor = _mentorService.GetById(course.MentorId);
        var mentorAccount = await _userService.GetUserById(mentor.UserId);
        return Ok(ApiResult<GetCourseDetailsResponse>.Succeed(new GetCourseDetailsResponse()
   
        {
            Schedule = course.Schedule.ToString(),
            Picture = course.Picture,
            SubjectName = subject.SubjectName,
            Duration = course.Duration,
            Price = course.Price,
            CourseName = course.CourseName,
            MentorName = mentorAccount.UserName,
            StartDate = course.StartDate,
            Avatar = mentorAccount.Avatar
        }));
    }

    [HttpGet]
    [Route(("relative-courses/{courseId:guid}"))]
    public async Task<IActionResult> GetRelativeCourses([FromRoute] Guid courseId)
    {
        var course = _courseService.GetCourseById(courseId);
        var result = _courseService.GetCourseByMentorId(course.MentorId, courseId);
        var listResult = new List<CourseResponse>();
        foreach (var courseModel in result)
        {
            var subject = _subjectService.GetSubjectById(courseModel.SubjectId);
            var mentor = _mentorService.GetById(courseModel.MentorId);
            var user =await _userService.GetUserById(mentor.UserId);
            listResult.Add(new CourseResponse()
            {
                Schedule = courseModel.Schedule.ToString(),
                CourseName = courseModel.CourseName,
                StartDate = courseModel.StartDate,
                Duration = courseModel.Duration,
                Price = courseModel.Price,
                Picture = courseModel.Picture,
                SubjectName = subject.SubjectName,
                MentorName = user.UserName,
                CourseId = courseModel.CourseId
            });
        }
        return Ok(ApiResult<GetRelativeCourseResponse>.Succeed(new GetRelativeCourseResponse()
        {
            ListRelativeCourse = listResult
        }));
    }



}