using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace EduViz.Entities
{

    [Table("Questions")]
    public class Question
    {
        [Key]
        public Guid QuestionId { get; set; }

        [Required]
        public string QuestionText { get; set; }

        [Required]
        public string AnswerA { get; set; }

        [Required]
        public string AnswerB { get; set; }

        public string? AnswerC { get; set; }
        public string? AnswerD { get; set; }

        [Required]
        public string CorrectAnswer { get; set; }

        [ForeignKey("Quiz")]
        public Guid QuizId { get; set; }

        public virtual Quiz Quiz { get; set; }
    }
}
