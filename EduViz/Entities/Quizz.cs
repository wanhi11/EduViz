using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace EduViz.Entities
{

    [Table("Quizzes")]
    public class Quiz
    {
        [Key]
        public Guid QuizId { get; set; }

        [Required]
        [MaxLength(200)]
        public string QuizTitle { get; set; }

        [ForeignKey("Class")]
        public Guid ClassId { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }

        public virtual Class Class { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<StudentQuizScore> StudentQuizScores { get; set; }
    }
}
