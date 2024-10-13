namespace EduViz.Common.Payloads.Response;

public class GetAllQuizByCourseResponse
{
    public List<QuizInCourse> quizzes { get; set; }
}

public class QuizInCourse
{
    public Guid quizId { get; set; }
    public string quizTitle { get; set; }
    public string duration { get; set; }
    public int totalStudent { get; set; }
    public int numOfStuAttempt { get; set; }
    public int numOfQuestion { get; set; }
}