using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace EduViz.Entities
{

    [Table("Questions")]
    public class Question
    {
        [Key]
        public Guid questionId { get; set; }

        [Required]
        [Column(TypeName = "NVARCHAR")]
        public string questionText { get; set; }

        [Required]
        [Column(TypeName = "NVARCHAR")]
        public string answerA { get; set; }

        [Required]
        [Column(TypeName = "NVARCHAR")]
        public string answerB { get; set; }
        
        [Column(TypeName = "NVARCHAR")]
        public string? answerC { get; set; }
        [Column(TypeName = "NVARCHAR")]
        public string? answerD { get; set; }
        
        public string? picture { get; set; }

        [Required]
        [Column(TypeName = "NVARCHAR")]
        public string correctAnswer { get; set; }

        [ForeignKey("Quiz")]
        public Guid quizId { get; set; }

        public virtual Quiz quiz { get; set; }
    }
}
