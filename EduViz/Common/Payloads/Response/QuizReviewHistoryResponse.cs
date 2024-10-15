namespace EduViz.Common.Payloads.Response;

public class QuizReviewHistoryResponse
{
    public Guid quizId { get; set; }
    public string quizTitle { get; set; }
    public TimeSpan duration { get; set; }
    public DateTime examDate { get; set; }
    public double score { get; set;}
    public List<QuestionWithStudentAnswer> resultList { get; set; }
}

public struct QuestionWithStudentAnswer
{
    public Guid questionId { get; set; }
    
    public string questionText { get; set; }

    public string answerA { get; set; }

    public string answerB { get; set; }

    public string? answerC { get; set; }

    public string? answerD { get; set; }
    public string correctAnswer { get; set; }
    public string studentAnswer { get; set; }
}