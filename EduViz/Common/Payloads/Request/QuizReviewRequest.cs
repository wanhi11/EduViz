namespace EduViz.Common.Payloads.Request;

public class QuizReviewRequest
{
    public Guid studentId { get; set; }
    public Guid scoreId { get; set; }
}