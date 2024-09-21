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
        public string questionText { get; set; }

        [Required]
        public string answerA { get; set; }

        [Required]
        public string answerB { get; set; }

        public string? answerC { get; set; }
        public string? answerD { get; set; }

        [Required]
        public string correctAnswer { get; set; }

        [ForeignKey("Quiz")]
        public Guid quizId { get; set; }

        public virtual Quiz quiz { get; set; }
    }
}
