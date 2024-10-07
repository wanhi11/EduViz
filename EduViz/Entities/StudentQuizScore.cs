using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace EduViz.Entities
{

    [Table("StudentQuizScores")]
    public class StudentQuizScore
    {
        [Key]
        public Guid studentQuizScoreId { get; set; }

        [ForeignKey("User")]
        public Guid userId { get; set; }

        [ForeignKey("Quiz")]
        public Guid quizId { get; set; }
        public TimeSpan duration { get; set; }     
        public DateTime dateTaken { get; set; }

        [Required]
        public double score { get; set; }

        public virtual User user { get; set; }
        public virtual Quiz quiz { get; set; }
    }
}
