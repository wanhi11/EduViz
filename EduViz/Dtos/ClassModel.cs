namespace EduViz.Dtos;

public class ClassModel
{
    public Guid classId { get; set; }
    
    public string className { get; set; }
    
    public Guid courseId { get; set; }
    
    public Guid mentorId { get; set; }
}