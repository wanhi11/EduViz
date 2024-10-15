using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduViz.Entities;

[Table("StudentAnswers")]
public class StudentAnswer
{
    [Key]
    public Guid studentAnswerId { get; set; }

    [ForeignKey("Quiz")]
    public Guid quizId { get; set; }

    [ForeignKey("Question")]
    public Guid questionId { get; set; }
    
    [ForeignKey("StudentQuizScores")]
    public Guid studentQuizScoreId { get; set; }

    public string selectedAnswer { get; set; } // Đáp án mà học viên đã chọn

    public virtual Quiz quiz { get; set; }
    public virtual Question question { get; set; }
    public virtual StudentQuizScore studentQuizScore { get; set; }
}