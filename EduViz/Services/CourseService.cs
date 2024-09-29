using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using EduViz.Common.Payloads.Response;
using EduViz.Dtos;
using EduViz.Entities;
using EduViz.Exceptions;
using EduViz.Helpers;
using EduViz.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EduViz.Services;

public class CourseService
{
    private readonly IRepository<User, Guid> _userRepository;
    private readonly IRepository<Subject, Guid> _subjectRepository;
    private readonly IMapper _mapper;
    private readonly IRepository<Course, Guid> _courseRepositoty;
    private readonly IRepository<MentorDetails, Guid> _mentorRepository;
    private readonly CloudinaryService _cloudinaryService;
    private readonly IRepository<Class, Guid> _classRepository;

    public CourseService(IRepository<User, Guid> userRepository, IRepository<Subject, Guid> subjectRepository,
        IMapper mapper, IRepository<Course, Guid> courseRepository, IRepository<MentorDetails, Guid> mentorRepository,
        CloudinaryService cloudinaryService,IRepository<Class,Guid> classRepository)
    {
        _userRepository = userRepository;
        _subjectRepository = subjectRepository;
        _mapper = mapper;
        _courseRepositoty = courseRepository;
        _mentorRepository = mentorRepository;
        _cloudinaryService = cloudinaryService;
        _classRepository = classRepository;
    }

    public async Task<CourseModel?> CreateCourse(CourseModel newCourse)
    {
        var subject = _subjectRepository.FindByCondition(s => s.subjectId.Equals(newCourse.SubjectId))
            .FirstOrDefault();

        if (subject is null)
        {
            throw new NotFoundException("Cannot find the suitable subject!");
        }

        var existedCourse = _courseRepositoty.FindByCondition(c => c.courseName.Equals(newCourse.CourseName))
            .FirstOrDefault();
        if (existedCourse is not null)
        {
            throw new BadRequestException("Course name has been existed");
        }

        var course = _mapper.Map<Course>(newCourse);

        await _courseRepositoty.AddAsync(course);

        if (await _courseRepositoty.Commit() > 0)
        {
            var newClass = new Class()
            {
                courseId = course.courseId,
                classId = course.courseId,
                className = course.courseName,
                mentorId = course.mentorId
            };
            await _classRepository.AddAsync(newClass);
            if (!(await _classRepository.Commit() > 0))
            {
                throw new BadRequestException("Can not create class");
            }

            return newCourse;
        }

        return null;
    }

    public async Task<List<CourseModel>?> GetCoursesBySubjectWithVipMentorFirst(string subjectName)
    {
        var currentDate = DateTime.UtcNow;

        var existedCourse = _courseRepositoty.FindByCondition(c => c.subject.subjectName.Equals(subjectName));
        if (!existedCourse.Any())
        {
            return null;
        }

        // Fetch all VIP mentors first
        var vipMentors = await _mentorRepository
            .FindByCondition(m => m.vipExpirationDate > currentDate)
            .ToListAsync();

        // Fetch all courses that have a start date greater than the current date
        var coursesBySubject = await existedCourse.Where(c => c.startDate > DateTime.Now)
            .ToListAsync();

        // Get a list of VIP Mentor Ids
        var vipMentorIds = vipMentors.Select(m => m.mentorDetailsId).ToList();

        // Separate VIP courses and non-VIP courses
        var vipCourses = coursesBySubject
            .Where(c => vipMentorIds.Contains(c.mentorId))
            .ToList();

        var nonVipCourses = coursesBySubject
            .Where(c => !vipMentorIds.Contains(c.mentorId))
            .ToList();

        // Combine both VIP and non-VIP courses
        var combinedCourses = vipCourses.Concat(nonVipCourses).ToList();

        // Map the combined courses to the desired model
        return _mapper.Map<List<CourseModel>>(combinedCourses);
    }


    public List<CourseModel> GetCourseByMentorId(Guid mentorId, Guid? currentCourseId)
    {
        var query = _courseRepositoty.FindByCondition(c => c.mentor.mentorDetailsId.Equals(mentorId));

        if (currentCourseId.HasValue)
        {
            query = query.Where(c => !c.courseId.Equals(currentCourseId.Value));
        }

        var listRelatedCourse = query.ToList();

        if (!listRelatedCourse.Any())
        {
            throw new NotFoundException("There is no Course available.");
        }

        return _mapper.Map<List<CourseModel>>(listRelatedCourse);
    }


    public CourseModel GetCourseById(Guid courseId)
    {
        var neededCourse = _courseRepositoty.FindByCondition(c => c.courseId.Equals(courseId)).FirstOrDefault();
        if (neededCourse is null)
        {
            throw new BadRequestException("There is no Course");
        }

        return _mapper.Map<CourseModel>(neededCourse);
    }

    public bool HasCourse()
    {
        var courses = _courseRepositoty.GetAll().ToList();
        if (courses.Any()) return true;
        return false;
    }

 public async Task<List<CourseResponse>> GetAllCoursesWithSearchString(string searchString)
{
    searchString = searchString.ToLower();
    var currentDate = DateTime.UtcNow;

    // Lấy danh sách các khóa học chứa searchString trong tên khóa học từ database
    var coursesList = await _courseRepositoty
        .FindByCondition(course => course.courseName.ToLower().Contains(searchString))
        .Include(course => course.mentor)
        .ThenInclude(mentor => mentor.user)
        .Include(course => course.subject)
        .ToListAsync();

    // Lấy danh sách các mentor có tên chứa searchString từ database
    var mentorsList = await _mentorRepository
        .FindByCondition(mentor => mentor.user.userName.ToLower().Contains(searchString))
        .Include(mentor => mentor.courses)
        .ThenInclude(course => course.subject)
        .ToListAsync();

    // Lấy danh sách các khóa học từ danh sách mentor đã tìm thấy
    var mentorCoursesList = mentorsList
        .SelectMany(mentor => mentor.courses)
        .ToList();

    // Kết hợp cả hai danh sách khóa học và loại bỏ trùng lặp bằng courseId
    var combinedCourses = coursesList
        .Concat(mentorCoursesList)
        .GroupBy(course => course.courseId)
        .Select(group => group.First())
        .ToList();

    // Chuyển đổi kết quả sang CourseResponse
    var courseResponses = combinedCourses.Select(course => new CourseResponse
    {
        courseId = course.courseId,
        courseName = course.courseName,
        mentorName = course.mentor.user.userName,
        subjectName = course.subject.subjectName,
        price = course.price,
        picture = course.picture,
        startDate = course.startDate,
        duration = course.duration,
        mentorId = course.mentorId.ToString(),
        weekSchedule = ConvertEnumHelper.ConvertEnumToDayList(course.schedule.ToString()), // Chuyển đổi schedule thành List<string>
        beginingClass = course.beginingClass.ToString(@"hh\:mm\:ss"), // Định dạng thời gian bắt đầu
        endingClass = course.endingClass.ToString(@"hh\:mm\:ss") // Định dạng thời gian kết thúc
    }).ToList();

    // Sắp xếp theo trạng thái VIP của mentor (VIP trước, không VIP sau)
    var sortedCourses = courseResponses
        .OrderByDescending(c => mentorsList
            .Any(m => m.mentorDetailsId == combinedCourses
                .Where(course => course.courseId == c.courseId)
                .Select(course => course.mentor.mentorDetailsId)
                .FirstOrDefault() && m.vipExpirationDate > currentDate))
        .ToList();

    return sortedCourses;
}

public async Task<List<CourseWithSubject>> GetCoursesGroupedBySubjectWithSearchString(string searchString)
{
    var currentDate = DateTime.UtcNow;

    // Sử dụng hàm đã có để lấy danh sách khóa học
    var combinedCourses = await GetAllCoursesWithSearchString(searchString);

    // Nhóm khóa học theo môn học
    var courseGroups = combinedCourses
        .GroupBy(course => course.subjectName)
        .Select(group => new CourseWithSubject
        {
            subjectName = group.Key,
            listCourse = group.ToList() // Chuyển nhóm thành danh sách
        })
        .ToList();

    return courseGroups; // Trả về danh sách CourseWithSubject
}

}