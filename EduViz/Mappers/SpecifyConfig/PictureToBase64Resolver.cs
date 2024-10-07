using AutoMapper;
using EduViz.Dtos;
using EduViz.Entities;

namespace EduViz.Mappers.SpecifyConfig;

public class PictureToBase64Resolver : IValueResolver<Question, QuestionModel, string>
{
    public string Resolve(Question source, QuestionModel destination, string destMember, ResolutionContext context)
    {
        // Nếu ảnh tồn tại, chuyển thành Base64, nếu không trả về null
        return source.picture != null ? Convert.ToBase64String(source.picture) : null;
    }
}