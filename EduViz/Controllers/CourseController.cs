using Azure.Core;
using EduViz.Common.Payloads;
using EduViz.Common.Payloads.Request;
using EduViz.Common.Payloads.Response;
using EduViz.Enums;
using EduViz.Exceptions;
using EduViz.Helpers;
using EduViz.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace EduViz.Controllers;

[ApiController]
[Route("api/course")]
public class CourseController : ControllerBase
{
    private readonly CourseService _courseService;
    private readonly SubjectService _subjectService;
    private readonly UserService _userService;
    private readonly MentorDetailService _mentorService;
    private readonly CloudinaryService _cloudinaryService;

    public CourseController(CourseService courseService, SubjectService subjectService,
        UserService userService, MentorDetailService mentorService, CloudinaryService cloudinaryService)
    {
        _courseService = courseService;
        _subjectService = subjectService;
        _userService = userService;
        _mentorService = mentorService;
        _cloudinaryService = cloudinaryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCourse()
    {
        if (!_courseService.HasCourse())
        {
            throw new NotFoundException("there is no course");
        }

        List<CourseWithSubject> result = new List<CourseWithSubject>();

        var listSubject = _subjectService.GetAllSubject();
        foreach (var subject in listSubject)
        {
            var courses = await _courseService.GetCoursesBySubjectWithVipMentorFirst(subject.SubjectName);
            if (courses is null)
            {
                result.Add(new CourseWithSubject()
                {
                    subjectName = subject.SubjectName,
                    listCourse = null
                });
                break;
            }

            List<CourseResponse> courseResponses = new List<CourseResponse>();
            foreach (var course in courses)
            {
                var mentor = _mentorService.GetById(course.MentorId);
                var user = _userService.GetUserById(mentor.UserId);
                courseResponses.Add(new CourseResponse()
                {
                    weekSchedule = ConvertEnumHelper.ConvertEnumToDayList(course.Schedule.ToString()),
                    courseName = course.CourseName,
                    startDate = course.StartDate,
                    duration = course.Duration,
                    price = course.Price,
                    picture = course.Picture,
                    subjectName = subject.SubjectName,
                    mentorName = user.UserName,
                    courseId = course.CourseId,
                    mentorId = course.MentorId.ToString(),
                    beginingClass = course.beginingClass.ToString(@"hh\:mm\:ss"),
                    endingClass = course.endingClass.ToString(@"hh\:mm\:ss")
                });
            }

            result.Add(new CourseWithSubject()
            {
                subjectName = subject.SubjectName,
                listCourse = courseResponses
            });
        }

        return Ok(ApiResult<GetAllCourseResponse>.Succeed(new GetAllCourseResponse()
        {
            listCourseWithSubjects = result
        }));
    }

    [HttpGet]
    [Route("/{subjectName}")]
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
            var user = _userService.GetUserById(mentor.UserId);
            listResult.Add(new CourseResponse()
            {
                weekSchedule = ConvertEnumHelper.ConvertEnumToDayList(course.Schedule.ToString()),
                courseName = course.CourseName,
                startDate = course.StartDate,
                duration = course.Duration,
                price = course.Price,
                picture = course.Picture,
                subjectName = subject.SubjectName,
                mentorName = user.UserName,
                courseId = course.CourseId,
                mentorId = course.MentorId.ToString(),
                beginingClass = course.beginingClass.ToString(@"hh\:mm\:ss"),
                endingClass = course.endingClass.ToString(@"hh\:mm\:ss")
            });
        }

        return Ok(ApiResult<GetAllCourseWithName>.Succeed(new GetAllCourseWithName()
        {
            listCourse = listResult
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
        var subject = _subjectService.GetSubjectByName(req.subjectName);
        course.SubjectId = subject.SubjectId;
        course.MentorId = mentor.MentorDetailsId;
        course.Picture = req.picture.IsNullOrEmpty() ? null : req.picture;
        var result = await _courseService.CreateCourse(course);
        if (course is null)
        {
            throw new BadRequestException("something went wrong when create course");
        }

        var response = new CourseResponse()
        {
            courseId = result!.CourseId,
            weekSchedule = ConvertEnumHelper.ConvertEnumToDayList(result.Schedule.ToString()),
            courseName = result.CourseName,
            startDate = result.StartDate,
            duration = result.Duration,
            price = result.Price,
            mentorName = currentUser.UserName,
            picture = result.Picture,
            subjectName = subject.SubjectName,
            mentorId = course.MentorId.ToString(),
            beginingClass = course.beginingClass.ToString(@"hh\:mm\:ss"),
            endingClass = course.endingClass.ToString(@"hh\:mm\:ss")
        };
        return Ok(ApiResult<CreateCourseResponse>.Succeed(new CreateCourseResponse()
        {
            createdCourse = response
        }));
    }

    [HttpGet]
    [Route("get-week-schedule")]
    public IActionResult GetWeekSchedule()
    {
        return Ok(ApiResult<GetWeekScheduleResponse>.Succeed(new GetWeekScheduleResponse()
        {
            weekSchedule = new List<List<string>>()
            {
                ConvertEnumHelper.ConvertEnumToDayList(Schedule.MonWedFri.ToString()),
                ConvertEnumHelper.ConvertEnumToDayList(Schedule.TueThuSat.ToString()),
                ConvertEnumHelper.ConvertEnumToDayList(Schedule.SatSun.ToString()),
            }
        }));
    }

    [HttpGet]
    [Route("detail/{courseId:guid}")]
    public IActionResult GetCourseDetail([FromRoute] Guid courseId)
    {
        var course = _courseService.GetCourseById(courseId);
        var subject = _subjectService.GetSubjectById(course.SubjectId);
        var mentor = _mentorService.GetById(course.MentorId);
        var mentorAccount = _userService.GetUserById(mentor.UserId);
        return Ok(ApiResult<GetCourseDetailsResponse>.Succeed(new GetCourseDetailsResponse()

        {
            weekSchedule = ConvertEnumHelper.ConvertEnumToDayList(course.Schedule.ToString()),
            picture = course.Picture,
            subjectName = subject.SubjectName,
            duration = course.Duration,
            price = course.Price,
            courseName = course.CourseName,
            mentorName = mentorAccount.UserName,
            startDate = course.StartDate,
            avatar = mentorAccount.Avatar,
            mentorId = course.MentorId.ToString(),
            beginingClass = course.beginingClass.ToString(@"hh\:mm\:ss"),
            endingClass = course.endingClass.ToString(@"hh\:mm\:ss")
        }));
    }

    [HttpGet]
    [Route(("relative-courses/{courseId:guid}"))]
    public IActionResult GetRelativeCourses([FromRoute] Guid courseId)
    {
        var course = _courseService.GetCourseById(courseId);
        var result = _courseService.GetCourseByMentorId(course.MentorId, courseId);
        var listResult = new List<CourseResponse>();
        foreach (var courseModel in result)
        {
            var subject = _subjectService.GetSubjectById(courseModel.SubjectId);
            var mentor = _mentorService.GetById(courseModel.MentorId);
            var user = _userService.GetUserById(mentor.UserId);
            listResult.Add(new CourseResponse()
            {
                weekSchedule = ConvertEnumHelper.ConvertEnumToDayList(courseModel.Schedule.ToString()),
                courseName = courseModel.CourseName,
                startDate = courseModel.StartDate,
                duration = courseModel.Duration,
                price = courseModel.Price,
                picture = courseModel.Picture,
                subjectName = subject.SubjectName,
                mentorName = user.UserName,
                courseId = courseModel.CourseId,
                mentorId = mentor.MentorDetailsId.ToString(),
                beginingClass = course.beginingClass.ToString(@"hh\:mm\:ss"),
                endingClass = course.endingClass.ToString(@"hh\:mm\:ss")
            });
        }

        return Ok(ApiResult<GetRelativeCourseResponse>.Succeed(new GetRelativeCourseResponse()
        {
            listRelativeCourse = listResult
        }));
    }

    [HttpGet("search/{searchString}")]
    public async Task<IActionResult> GetAllCoursesWithSearchString([FromRoute] string searchString)
    {
        var courses = await _courseService.GetCoursesGroupedBySubjectWithSearchString(searchString);

        if (courses is null || !courses.Any())
        {
            throw new NotFoundException("No courses or mentors found matching the provided search string.");
        }

        ;


        return Ok(ApiResult<GetAllCourseResponse>.Succeed(new GetAllCourseResponse()
        {
            listCourseWithSubjects = courses
        }));
    }

    [HttpGet("{mentorId:guid}/get-by-mentor")]
    public IActionResult GetAllCourseByMentor([FromRoute] Guid mentorId)
    {
        var result = _courseService.GetCourseByMentorId(mentorId,null);
        var listResult = new List<CourseResponse>();
        foreach (var courseModel in result)
        {
            var subject = _subjectService.GetSubjectById(courseModel.SubjectId);
            var mentor = _mentorService.GetById(courseModel.MentorId);
            var user = _userService.GetUserById(mentor.UserId);
            listResult.Add(new CourseResponse()
            {
                weekSchedule = ConvertEnumHelper.ConvertEnumToDayList(courseModel.Schedule.ToString()),
                courseName = courseModel.CourseName,
                startDate = courseModel.StartDate,
                duration = courseModel.Duration,
                price = courseModel.Price,
                picture = courseModel.Picture,
                subjectName = subject.SubjectName,
                mentorName = user.UserName,
                courseId = courseModel.CourseId,
                mentorId = mentor.MentorDetailsId.ToString(),
                beginingClass = courseModel.beginingClass.ToString(@"hh\:mm\:ss"),
                endingClass = courseModel.endingClass.ToString(@"hh\:mm\:ss")
            });
        }

        return Ok(ApiResult<GetRelativeCourseResponse>.Succeed(new GetRelativeCourseResponse()
        {
            listRelativeCourse = listResult
        }));
    }

}