using AutoMapper;
using EduViz.Dtos;
using EduViz.Entities;
using EduViz.Exceptions;
using EduViz.Repositories;

namespace EduViz.Controllers;

public class SubjectService
{
    private readonly IRepository<Subject, Guid> _subjectRepository;
    private readonly IMapper _mapper;

    public SubjectService(IRepository<Subject,Guid> subjectRepository, IMapper mapper)
    {
        _subjectRepository = subjectRepository;
        _mapper = mapper;
    }

    public List<SubjectModel> GetAllSubject()
    {
        var result = _subjectRepository.GetAll().ToList();
        if (!result.Any())
        {
            throw new NotFoundException("There is no course");
        }

        return _mapper.Map<List<SubjectModel>>(result);
    }
    
}