using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EduViz.Entities
{
    [Table("Questions")]
    public class Question
    {
        [Key] public Guid questionId { get; set; }

        [Required]
        [MaxLength(500)] // Max length cho câu hỏi
        [Column(TypeName = "NVARCHAR")]
        public string questionText { get; set; }

        [Required]
        [MaxLength(100)] // Max length cho câu trả lời A
        [Column(TypeName = "NVARCHAR")]
        public string answerA { get; set; }

        [Required]
        [MaxLength(100)] // Max length cho câu trả lời B
        [Column(TypeName = "NVARCHAR")]
        public string answerB { get; set; }

        [MaxLength(100)] // Max length cho câu trả lời C
        [Column(TypeName = "NVARCHAR")]
        public string? answerC { get; set; }

        [MaxLength(100)] // Max length cho câu trả lời D
        [Column(TypeName = "NVARCHAR")]
        public string? answerD { get; set; }

        public byte[]? picture { get; set; }

        [Required]
        [MaxLength(1)] // Max length cho đáp án đúng
        [Column(TypeName = "NVARCHAR")]
        public string correctAnswer { get; set; }

        [ForeignKey("Quiz")] public Guid quizId { get; set; }

        public virtual Quiz quiz { get; set; }
        public virtual ICollection<StudentAnswer> studentAnswers { get; set; }
    }
}