using EduViz.Dtos;

namespace EduViz.Common.Payloads.Response;

public class GetQuizHistoryWithExactCourseResponse
{
    public List<StudentQuizScoreModel> quizHistory { get; set; }
}