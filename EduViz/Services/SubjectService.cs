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

    public SubjectModel GetSubjectByName(string name)
    {
        var result = _subjectRepository.FindByCondition(s => s.subjectName.Equals(name)).FirstOrDefault();
        if (result is null)
        {
            throw new NotFoundException("there is no suitable subject");
        }

        return _mapper.Map<SubjectModel>(result);
    }

    public SubjectModel GetSubjectById(Guid subjectId)
    {
        var subject = _subjectRepository.FindByCondition(s => s.subjectId.Equals(subjectId)).FirstOrDefault();
        if (subject is null)
        {
            throw new NotFoundException("there is no suitable subject");
        }

        return _mapper.Map<SubjectModel>(subject);
    }

}