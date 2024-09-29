using AutoMapper;
using EduViz.Dtos;
using EduViz.Entities;
using EduViz.Repositories;

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
}