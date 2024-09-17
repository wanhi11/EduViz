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

    public CourseService(IRepository<User, Guid> userRepository, IRepository<Subject, Guid> subjectRepository,
        IMapper mapper,IRepository<Course,Guid> courseRepository,IRepository<MentorDetails,Guid> mentorRepository)
    {
        _userRepository = userRepository;
        _subjectRepository = subjectRepository;
        _mapper = mapper;
        _courseRepositoty = courseRepository;
        _mentorRepository = mentorRepository;
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
        
        var vipMentors = await _mentorRepository
            .FindByCondition(m => m.VipExpirationDate > currentDate)
            .OrderBy(m => Guid.NewGuid())
            .ToListAsync();
        
        var coursesBySubject = _courseRepositoty
            .FindByCondition(c => c.Subject.SubjectName.Equals(subjectName, StringComparison.OrdinalIgnoreCase));
        
        var vipCourses = coursesBySubject
            .Where(c => vipMentors.Any(m => m.MentorDetailsId == c.MentorId))
            .ToList();
        
        var nonVipCourses = coursesBySubject
            .Where(c => !vipMentors.Any(m => m.MentorDetailsId == c.MentorId))
            .ToList();
        
        var combinedCourses = vipCourses.Concat(nonVipCourses).ToList();

        return _mapper.Map<List<CourseModel>>(combinedCourses);
    }
    public async Task<List<CourseModel>> GetCoursesWithVipMentorFirst()
    {
        var currentDate = DateTime.UtcNow;
        
        var vipMentors = await _mentorRepository
            .FindByCondition(m => m.VipExpirationDate > currentDate)
            .OrderBy(m => Guid.NewGuid())
            .ToListAsync();

        var coursesBySubject = _courseRepositoty
            .GetAll();
        
        var vipCourses = coursesBySubject
            .Where(c => vipMentors.Any(m => m.MentorDetailsId == c.MentorId))
            .ToList();
        
        var nonVipCourses = coursesBySubject
            .Where(c => !vipMentors.Any(m => m.MentorDetailsId == c.MentorId))
            .ToList();
        
        var combinedCourses = vipCourses.Concat(nonVipCourses).ToList();

        return _mapper.Map<List<CourseModel>>(combinedCourses);
    }

}