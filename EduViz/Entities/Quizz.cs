using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace EduViz.Entities
{

    [Table("Quizzes")]
    public class Quiz
    {
        [Key]
        public Guid quizId { get; set; }

        [Required]
        [MaxLength(200)]
        [Column(TypeName = "NVARCHAR")]
        public string quizTitle { get; set; }

        [ForeignKey("Class")]
        public Guid classId { get; set; }

        [Required]
        public TimeSpan duration { get; set; }

        public virtual Class mentorClass { get; set; }
        public virtual ICollection<Question> questions { get; set; }
        public virtual ICollection<StudentQuizScore> studentQuizScores { get; set; }
        public virtual ICollection<StudentAnswer> studentAnswers { get; set; }
    }
}
