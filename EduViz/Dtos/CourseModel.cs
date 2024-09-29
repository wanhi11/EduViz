using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EduViz.Enums;

namespace EduViz.Dtos;

public class CourseModel
{
    public Guid CourseId { get; set; }
    
    public string CourseName { get; set; }
    
    public Guid MentorId { get; set; }
    
    public Guid SubjectId { get; set; }
    
    public decimal Price { get; set; }
    public string? Picture { get; set; }
    public string meetUrl { get; set; }
    public DateTime StartDate { get; set; }
    public int Duration { get; set; }
            
    public Schedule Schedule { get; set; }
    public TimeSpan beginingClass { get; set; }
    public TimeSpan endingClass { get; set; }
}