using EduViz.Enums;

namespace EduViz.Common.Payloads.Response;

public class GetCourseDetailsResponse
{
    public string CourseName { get; set; }
    public decimal Price { get; set; }
    public string? Picture { get; set; }
            
    public DateTime StartDate { get; set; }
    public int Duration { get; set; }
            
    public string Schedule { get; set; }
    public string SubjectName { get; set; }
    public string MentorName { get; set; }
    public string Avatar { get; set; }
}