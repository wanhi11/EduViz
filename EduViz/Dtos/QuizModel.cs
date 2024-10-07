namespace EduViz.Dtos;

public class QuizModel
{
    public Guid quizId { get; set; }
    
    public string quizTitle { get; set; }
    
    public Guid classId { get; set; }
    
    public TimeSpan duration { get; set; }
}