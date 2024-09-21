using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using EduViz.Dtos;
using EduViz.Entities;
using EduViz.Exceptions;
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

    public CourseService(IRepository<User, Guid> userRepository, IRepository<Subject, Guid> subjectRepository,
        IMapper mapper, IRepository<Course, Guid> courseRepository, IRepository<MentorDetails, Guid> mentorRepository,
        CloudinaryService cloudinaryService)
    {
        _userRepository = userRepository;
        _subjectRepository = subjectRepository;
        _mapper = mapper;
        _courseRepositoty = courseRepository;
        _mentorRepository = mentorRepository;
        _cloudinaryService = cloudinaryService;
    }

    public async Task<CourseModel?> CreateCourse(CourseModel newCourse)
    {
        var subject = _subjectRepository.FindByCondition(s => s.SubjectName.Equals(newCourse.CourseName))
            .FirstOrDefault();
        if (subject is null)
        {
            throw new NotFoundException("Cannot find the suitable subject!");
        }
        
        var user = _userRepository.FindByCondition(u => u.UserId.Equals(newCourse.MentorId)).FirstOrDefault();
        if (user is null)
        {
            throw new NotFoundException("This account has been removed or banned");
        }
        var course = _mapper.Map<Course>(newCourse);

        await _courseRepositoty.AddAsync(course);

        if (await _courseRepositoty.Commit() > 0)
        {
            return newCourse;
        }

        return null;
    }
    public async Task<List<CourseModel>> GetCoursesBySubjectWithVipMentorFirst(string subjectName)
    {
        var currentDate = DateTime.UtcNow;

        // Fetch all VIP mentors first
        var vipMentors = await _mentorRepository
            .FindByCondition(m => m.VipExpirationDate > currentDate)
            .ToListAsync();

        // Fetch all courses that have a start date greater than the current date
        var coursesBySubject = await _courseRepositoty
            .FindByCondition(c => c.StartDate > currentDate && c.Subject.SubjectName.Equals(subjectName))
            .ToListAsync();

        // Get a list of VIP Mentor Ids
        var vipMentorIds = vipMentors.Select(m => m.MentorDetailsId).ToList();

        // Separate VIP courses and non-VIP courses
        var vipCourses = coursesBySubject
            .Where(c => vipMentorIds.Contains(c.MentorId))
            .ToList();

        var nonVipCourses = coursesBySubject
            .Where(c => !vipMentorIds.Contains(c.MentorId))
            .ToList();

        // Combine both VIP and non-VIP courses
        var combinedCourses = vipCourses.Concat(nonVipCourses).ToList();

        // Map the combined courses to the desired model
        return _mapper.Map<List<CourseModel>>(combinedCourses);
    }

    public async Task<List<CourseModel>> GetCoursesWithVipMentorFirst()
    {
        var currentDate = DateTime.UtcNow;

        // Fetch all VIP mentors first
        var vipMentors = await _mentorRepository
            .FindByCondition(m => m.VipExpirationDate > currentDate)
            .ToListAsync();

        // Fetch all courses that have a start date greater than the current date
        var coursesBySubject = await _courseRepositoty
            .FindByCondition(c => c.StartDate > currentDate)
            .ToListAsync();

        // Get a list of VIP Mentor Ids
        var vipMentorIds = vipMentors.Select(m => m.MentorDetailsId).ToList();

        // Separate VIP courses and non-VIP courses
        var vipCourses = coursesBySubject
            .Where(c => vipMentorIds.Contains(c.MentorId))
            .ToList();

        var nonVipCourses = coursesBySubject
            .Where(c => !vipMentorIds.Contains(c.MentorId))
            .ToList();

        // Combine both VIP and non-VIP courses
        var combinedCourses = vipCourses.Concat(nonVipCourses).ToList();

        // Map the combined courses to the desired model
        return _mapper.Map<List<CourseModel>>(combinedCourses);
    }


    public List<CourseModel> GetCourseByMentorId(Guid mentorId,Guid currentCourseId)
    {
        var listRelatedCourse = _courseRepositoty.FindByCondition(c 
                => c.Mentor.MentorDetailsId.Equals(mentorId) && !c.CourseId.Equals(currentCourseId))
            .ToList();
        if (!listRelatedCourse.Any())
        {
            throw new NotFoundException("There is no Course else");
        }

        return _mapper.Map<List<CourseModel>>(listRelatedCourse);
    }

    public CourseModel GetCourseById(Guid courseId)
    {
        var neededCourse = _courseRepositoty.FindByCondition(c => c.CourseId.Equals(courseId)).FirstOrDefault();
        if (neededCourse is null)
        {
            throw new BadRequestException("There is no Course");
        }

        return _mapper.Map<CourseModel>(neededCourse);
    }
    


}