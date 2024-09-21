using EduViz.Dtos;
using EduViz.Entities;
using EduViz.Enums;

namespace EduViz.Common.Payloads.Response;

public class GetAllCourseResponse
{
    public List<CourseResponse> ListCourse { get; set; }
}

public class CourseResponse
{
    public Guid CourseId { get; set; }
    
    public string CourseName { get; set; }
    
    public string MentorName { get; set; }
    
    public string SubjectName { get; set; }
    
    public decimal Price { get; set; }
    public string? Picture { get; set; }
            
    public DateTime StartDate { get; set; }
    public int Duration { get; set; }
            
    public string Schedule { get; set; }
}
