using EduViz.Dtos;

namespace EduViz.Common.Payloads.Response;

public class QuizzDetailResponse
{
    public Guid quizId { get; set; }
    public string quizTitle { get; set; }
    public List<QuestionModel> questionList { get; set; }
}