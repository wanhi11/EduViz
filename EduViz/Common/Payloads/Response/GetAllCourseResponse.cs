using EduViz.Dtos;
using EduViz.Entities;
using EduViz.Enums;

namespace EduViz.Common.Payloads.Response;

public class GetAllCourseResponse
{
    public List<CourseResponse> listCourse { get; set; }
}

public class CourseResponse
{
    public Guid courseId { get; set; }
    
    public string courseName { get; set; }
    
    public string mentorName { get; set; }
    
    public string subjectName { get; set; }
    
    public decimal price { get; set; }
    public string? picture { get; set; }
            
    public DateTime startDate { get; set; }
    public int duration { get; set; }
            
    public string schedule { get; set; }
}
