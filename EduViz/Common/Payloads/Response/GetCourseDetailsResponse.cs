using EduViz.Enums;

namespace EduViz.Common.Payloads.Response;

public class GetCourseDetailsResponse
{
    public string courseName { get; set; }
    public decimal price { get; set; }
    public string? picture { get; set; }
    public string userId { get; set; }
    public DateTime startDate { get; set; }
    public string mentorId { get; set; }
    public int duration { get; set; }
    public int numOfStudents { get; set; }
            
    public List<string> weekSchedule { get; set; }
    public string subjectName { get; set; }
    public string mentorName { get; set; }
    public string avatar { get; set; }
    public string beginingClass { get; set; }
    public string endingClass { get; set; }
}