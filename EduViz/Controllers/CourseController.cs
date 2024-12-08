using Azure.Core;
using EduViz.Common.Payloads;
using EduViz.Common.Payloads.Request;
using EduViz.Common.Payloads.Response;
using EduViz.Entities;
using EduViz.Enums;
using EduViz.Exceptions;
using EduViz.Helpers;
using EduViz.Repositories;
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
    private readonly ClassService _classService;
    private readonly QuizService _quizService;

    public CourseController(CourseService courseService, SubjectService subjectService,
        UserService userService, MentorDetailService mentorService, CloudinaryService cloudinaryService,
        ClassService classService,QuizService quizService)
    {
        _courseService = courseService;
        _subjectService = subjectService;
        _userService = userService;
        _mentorService = mentorService;
        _cloudinaryService = cloudinaryService;
        _classService = classService;
        _quizService = quizService;
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
                    listCourse = new List<CourseResponse>()
                });
                continue;
            }

            List<CourseResponse> courseResponses = new List<CourseResponse>();
            foreach (var course in courses)
            {
                int numOfStudent = _classService.GetNumbOfStuByClassId(course.CourseId);
                var mentor = _mentorService.GetById(course.MentorId);
                var user = _userService.GetUserById(mentor.UserId);
                courseResponses.Add(new CourseResponse()
                {
                    meetUrl = course.meetUrl,
                    userId = user.UserId.ToString(),
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
                    endingClass = course.endingClass.ToString(@"hh\:mm\:ss"),
                    numOfStudents = numOfStudent
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
            int numOfStudent = _classService.GetNumbOfStuByClassId(course.CourseId);
            var subject = _subjectService.GetSubjectById(course.SubjectId);
            var mentor = _mentorService.GetById(course.MentorId);
            var user = _userService.GetUserById(mentor.UserId);
            listResult.Add(new CourseResponse()
            {
                meetUrl = course.meetUrl,
                userId = user.UserId.ToString(),
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
                endingClass = course.endingClass.ToString(@"hh\:mm\:ss"),
                numOfStudents = numOfStudent
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
    public async Task<IActionResult> CreateNewCourse([FromBody] CreateCourseRequest req)
    {
        if(!ModelState.IsValid)
        {
            var errorMessage = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            throw new BadRequestException(string.Join("; ", errorMessage));
        }
        Request.Headers.TryGetValue("Authorization", out var token);
        token = token.ToString().Split()[1];
        var currentUser = _userService.GetUserInToken(token);
        var mentor = _mentorService.GetByMentorId(currentUser.UserId);
        var course = req.ToCourseModel();
        var subject = _subjectService.GetSubjectByName(req.subjectName);
        course.SubjectId = subject.SubjectId;
        course.MentorId = mentor.MentorDetailsId;
        // var uploadResult = await _cloudinaryService.UploadImageAsync(req.picture);
        //
        // if (uploadResult.Error != null)
        // {
        //     throw new BadRequestException("Failed to upload image.");
        // }
        //
        // course.Picture =uploadResult.SecureUrl.ToString();
        course.Picture = req.picture;
       
        var result = await _courseService.CreateCourse(course);
        if (course is null)
        {
            throw new BadRequestException("something went wrong when create course");
        }

        var response = new CourseResponse()
        {
            meetUrl = course.meetUrl,
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
        int numOfStudent = _classService.GetNumbOfStuByClassId(course.CourseId);
        return Ok(ApiResult<GetCourseDetailsResponse>.Succeed(new GetCourseDetailsResponse()

        {
            meetUrl = course.meetUrl,
            userId = mentorAccount.UserId.ToString(),
            numOfStudents = numOfStudent,
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
            int numOfStudent = _classService.GetNumbOfStuByClassId(course.CourseId);
            var subject = _subjectService.GetSubjectById(courseModel.SubjectId);
            var mentor = _mentorService.GetById(courseModel.MentorId);
            var user = _userService.GetUserById(mentor.UserId);
            listResult.Add(new CourseResponse()
            {
                meetUrl = courseModel.meetUrl,
                userId = user.UserId.ToString(),
                numOfStudents = numOfStudent,
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

    [HttpGet("mentor/{mentorId:guid}")]
    public IActionResult GetAllCourseByMentor([FromRoute] Guid mentorId)
    {
        var result = _courseService.GetCourseByMentorId(mentorId,null);
        var listResult = new List<CourseResponse>();
        foreach (var courseModel in result)
        {
            int numOfStudent = _classService.GetNumbOfStuByClassId(courseModel.CourseId);
            var subject = _subjectService.GetSubjectById(courseModel.SubjectId);
            var mentor = _mentorService.GetById(courseModel.MentorId);
            var user = _userService.GetUserById(mentor.UserId);
            listResult.Add(new CourseResponse()
            {
                meetUrl = courseModel.meetUrl,
                userId = user.UserId.ToString(),
                numOfStudents = numOfStudent,
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

    [HttpGet("class-detail/{classId:guid}")]
    public IActionResult GetClassDetails([FromRoute] Guid classId)
    {
        var classInfo = _classService.GetStudentsInClass(classId);
        var courseInfo = _courseService.GetCourseById(classId);
        var subject = _subjectService.GetSubjectById(courseInfo.SubjectId);
        return Ok(ApiResult<GetClassDetailsReponse>.Succeed(new GetClassDetailsReponse()
        {
            meetUrl = courseInfo.meetUrl,
            beginingClass = courseInfo.beginingClass.ToString(@"hh\:mm\:ss"),
            endingClass = courseInfo.endingClass.ToString(@"hh\:mm\:ss"),
            subjectName = subject.SubjectName,
            monthDuration = courseInfo.Duration,
            weekSchedule = ConvertEnumHelper.ConvertEnumToDayList(courseInfo.Schedule.ToString()),
            studentInfoList = classInfo,
        }));
    }
    [HttpGet("{classId:guid}/quizzes")]
    public async Task<IActionResult> GetAllQuizByCourse([FromRoute] Guid classId)
    {
        var result = await _quizService.GetAllQuizzesByCourse(classId);
        
        if (!result.quizzes.Any())
        {
            throw new BadRequestException("There is no Quiz yet");
        }
        return Ok(ApiResult<GetAllQuizByCourseResponse>.Succeed(result));
    }

    [HttpGet("mentor/{mentorId:guid}/quizzes")]
    public async Task<IActionResult> GetAllQuizByMentor([FromRoute] Guid mentorId)
    {
        var result = await _quizService.GetAllQuizzesByMentor(mentorId);
        if (!result.quizzes.Any())
        {
            throw new BadRequestException("There is no Quiz yet");
        }
        return Ok(ApiResult<GetAllQuizByCourseResponse>.Succeed(result));
    }

    [HttpGet("student/{studentId:guid}")]
    [Authorize(Roles = "Student")]
    public IActionResult GetAllPaidCourseOfStudent([FromRoute] Guid studentId)
    {
        var courses = _courseService.GetCourseByStudentId(studentId);
        if(courses is null) throw new BadRequestException("You have joined no courses");        var listResult = new List<CourseResponse>();
        foreach (var courseModel in courses)
        {
            int numOfStudent = _classService.GetNumbOfStuByClassId(courseModel.CourseId);
            var subject = _subjectService.GetSubjectById(courseModel.SubjectId);
            var mentor = _mentorService.GetById(courseModel.MentorId);
            var user = _userService.GetUserById(mentor.UserId);
            listResult.Add(new CourseResponse()
            {
                meetUrl = courseModel.meetUrl,
                userId = user.UserId.ToString(),
                numOfStudents = numOfStudent,
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

    [HttpPost]
    [Route("{classId:guid}/comment")]
    public async Task<IActionResult> AddCommentForCourse([FromRoute] Guid classId,[FromBody] CommentRequest req)
    {
        Request.Headers.TryGetValue("Authorization", out var token);
        token = token.ToString().Split()[1];
        var currentUser = _userService.GetUserInToken(token);
        var result = await _courseService.AddCommentForCourse(req.comment,req.rate, currentUser.UserId,
            classId);
        return Ok(ApiResult<CommentResponse>.Succeed(new CommentResponse()
        {
            userName = result.userName,
            comment = result.comment
        }));
    }
    
    [HttpGet]
    [Route("{classId:guid}/comment")]
    public async Task<IActionResult> GetComments([FromRoute] Guid classId)
    {
        var reult = await _courseService.GetCommentsOfClass(classId);
        return Ok(ApiResult<GetCommentResponse>.Succeed(new GetCommentResponse()
        {
            feedBacks = reult
        }));
    }
}