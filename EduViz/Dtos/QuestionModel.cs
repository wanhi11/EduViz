namespace EduViz.Dtos;

public class QuestionModel
{
    public Guid questionId { get; set; }
    
    public string questionText { get; set; }

    public string answerA { get; set; }

    public string answerB { get; set; }

    public string? answerC { get; set; }

    public string? answerD { get; set; }
        
    public string? picture { get; set; }

    public string correctAnswer { get; set; }
}