using EduViz.Dtos;

namespace EduViz.Common.Payloads.Response;

public class UploadQuestionsReponse
{
    public string quizId { get; set; }
    public string quizTitle { get; set; }
    public TimeSpan duration { get; set; }
    public int duplicationCount { get; set; }
    public List<QuestionModel> savedQuestions { get; set; }
}