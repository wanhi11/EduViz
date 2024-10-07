using EduViz.Dtos;
using EduViz.Exceptions;

namespace EduViz.Common.Payloads.Request;

public class UploadQuestionsRequest
{
    public string quizTitle { get; set; }
    public string duration { get; set; }
    public string courseId { get; set; }
    public IFormFile file { get; set; }
    
}

public static class UploadQuestionsRequestExtension
{
    public static QuizModel ToQuizModel(this UploadQuestionsRequest req)
    {
        TimeSpan duration;
        bool parseDuration = TimeSpan.TryParseExact(req.duration,"hh\\:mm\\:ss",null,out duration);
        if(!parseDuration) throw new BadRequestException("Format time must be hh:mm:ss");
        var quizModel = new QuizModel()
        {
            quizId = Guid.NewGuid(),
            duration = duration,
            quizTitle = req.quizTitle,
            classId = Guid.Parse(req.courseId)
        };
        return quizModel;
    }
}