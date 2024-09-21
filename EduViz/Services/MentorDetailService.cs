using AutoMapper;
using EduViz.Dtos;
using EduViz.Entities;
using EduViz.Repositories;

namespace EduViz.Services;

public class MentorDetailService
{
    private readonly IMapper _mapper;
    private readonly IRepository<MentorDetails, Guid> _mentorDetailRepository;

    public MentorDetailService(IMapper mapper, IRepository<MentorDetails,Guid> mentorDetailRepository)
    {
        _mapper = mapper;
        _mentorDetailRepository = mentorDetailRepository;
    }

    public  MentorDetailModel GetById(Guid Id)
    {
        return _mapper.Map<MentorDetailModel>(_mentorDetailRepository.FindByCondition(md =>
            md.mentorDetailsId.Equals(Id)).FirstOrDefault());
    }
    public  MentorDetailModel GetByMentorId(Guid MId)
    {
        return _mapper.Map<MentorDetailModel>(_mentorDetailRepository.FindByCondition(md =>
            md.userId.Equals(MId)).FirstOrDefault());
    }
}