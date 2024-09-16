using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduViz.Dtos;

public class CourseModel
{
    public Guid CourseId { get; set; }
    
    public string CourseName { get; set; }
    
    public Guid MentorId { get; set; }
    
    public Guid SubjectId { get; set; }
    
    public decimal Price { get; set; }
}