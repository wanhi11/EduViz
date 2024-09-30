using AutoMapper;
using EduViz.Dtos;
using EduViz.Entities;
using EduViz.Helpers;

namespace EduViz.Mappers;

public class ApplicationMapper : Profile
{
    public ApplicationMapper()
    {
        CreateMap<User, UserModel>().ReverseMap();
        CreateMap<Payment, PaymentModel>().ReverseMap();
        CreateMap<MentorDetails, MentorDetailModel>().ReverseMap();
        CreateMap<Course, CourseModel>().ReverseMap();
        CreateMap<UpgradeOrderDetails, UpgradeOrderDetailModel>().ReverseMap();
        CreateMap<Subject, SubjectModel>().ReverseMap();
        CreateMap<Class, ClassModel>().ReverseMap();
    }
}