using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace EduViz.Entities
{

    [Table("StudentQuizScores")]
    public class StudentQuizScore
    {
        [Key]
        public Guid StudentQuizScoreId { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [ForeignKey("Quiz")]
        public Guid QuizId { get; set; }

        [Required]
        public double Score { get; set; }

        public virtual User User { get; set; }
        public virtual Quiz Quiz { get; set; }
    }
}
