using EduViz.Dtos;
using EduViz.Entities;
using EduViz.Exceptions;

namespace EduViz.Common.Payloads.Request;

public class QuizSubmitRequest
{
    public Guid studentId { get; set; }
    public Guid quizId { get; set; }
    public string duration { get; set; }     
    public List<QuestionWithAnswer> answerList { get; set; }
}

public static class QuizSubmitRequestExtension
{
    public static StudentQuizScoreModel ToScoreModel(this QuizSubmitRequest req)
    {
        TimeSpan duration;
        bool isDuration = TimeSpan.TryParseExact(req.duration, "hh\\:mm\\:ss", null, out duration);
        if (!isDuration)
        {
            throw new BadRequestException("Duration must have the format hh:mm:ss");
        }

        var score = new StudentQuizScoreModel()
        {
            duration = duration,
            quizId = req.quizId,
            dateTaken = DateTime.Now,
            userId = req.studentId,
        };
        return score;
    }
}

public class QuestionWithAnswer
{
    public Guid questionId { get; set; }
    public string chosenAnswer { get; set; }
}