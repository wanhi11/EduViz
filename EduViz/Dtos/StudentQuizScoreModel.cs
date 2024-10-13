namespace EduViz.Dtos;

public class StudentQuizScoreModel
{
    public Guid studentQuizScoreId { get; set; }
    
    public Guid userId { get; set; }
    public Guid quizId { get; set; }
    public TimeSpan duration { get; set; }     
    public DateTime dateTaken { get; set; }
    public double score { get; set; }
}