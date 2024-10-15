namespace EduViz.Common.Payloads.Request;

public class GetQuizHistoryWithExactCourseRequest
{
    public Guid studentId { get; set; }
    public Guid courseId { get; set; }
}