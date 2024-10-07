using AutoMapper;
using EduViz.Dtos;
using EduViz.Entities;
using EduViz.Helpers;
using EduViz.Mappers.SpecifyConfig;

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
        CreateMap<Quiz, QuizModel>().ReverseMap();
        CreateMap<Question, QuestionModel>()
            .ForMember(dest => dest.picture, opt => opt.MapFrom<PictureToBase64Resolver>());
    }
}