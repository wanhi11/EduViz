using AutoMapper;
using EduViz.Common.Payloads.Response;
using EduViz.Dtos;
using EduViz.Entities;
using EduViz.Exceptions;
using EduViz.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EduViz.Services;

public class ClassService
{
    private readonly IRepository<Class, Guid> _classRepository;
    private readonly IMapper _mapper;
    private readonly IRepository<StudentClass, Guid> _studentClassRepository;

    public ClassService(IRepository<Class, Guid> classRepository,IMapper mapper,
        IRepository<StudentClass,Guid> studentClassRepository)
    {
        _classRepository = classRepository;
        _mapper = mapper;
        _studentClassRepository = studentClassRepository;
    }

    public int GetNumbOfStuByClassId(Guid classId)
    {
        int result =_studentClassRepository.FindByCondition(c => c.classId.ToString().ToLower()==classId.ToString().ToLower()).ToList().Count();
        return result;
    }

    public List<StudentInfoInClass> GetStudentsInClass(Guid classId)
    {
        var studentsInClass = _classRepository
            .FindByCondition(c => c.classId == classId)
            .Include(c => c.studentClasses)
            .ThenInclude(sc => sc.user)
            .Include(c => c.quizzes) // Lấy danh sách quiz của lớp
            .ThenInclude(q => q.studentQuizScores)
            .SelectMany(c => c.studentClasses.Select(sc => new StudentInfoInClass
            {
                studentId = sc.user.userId.ToString(),
                name = sc.user.userName,
                gender = sc.user.gender.HasValue ? sc.user.gender.Value.ToString() : "",
                // Tính toán số lần làm bài (numOfTry)
                numOfTry = c.quizzes
                    .SelectMany(q => q.studentQuizScores)
                    .Count(sqs => sqs.userId == sc.user.userId),
                // Tính toán điểm trung bình (score)
                score = c.quizzes
                    .SelectMany(q => q.studentQuizScores)
                    .Where(sqs => sqs.userId == sc.user.userId)
                    .Average(sqs => sqs.score)
            }))
            .ToList();
        return studentsInClass;
    }
}