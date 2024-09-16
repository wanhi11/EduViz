using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using EduViz.Dtos;
using EduViz.Entities;
using EduViz.Exceptions;
using EduViz.Repositories;

namespace EduViz.Services;

public class CourseService
{
    private readonly IRepository<User, Guid> _userRepository;
    private readonly IRepository<Subject, Guid> _subjectRepository;
    private readonly IMapper _mapper;
    private readonly IRepository<Course, Guid> _courseRepositoty;

    public CourseService(IRepository<User, Guid> userRepository, IRepository<Subject, Guid> subjectRepository,
        IMapper mapper,IRepository<Course,Guid> courseRepository)
    {
        _userRepository = userRepository;
        _subjectRepository = subjectRepository;
        _mapper = mapper;
        _courseRepositoty = courseRepository;
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

}