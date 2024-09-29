using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace EduViz.Entities
{
    [Table("Classes")]
    public class Class
    {
        [Key]
        public Guid classId { get; set; }

        [Required]
        [MaxLength(150)]
        [Column(TypeName = "NVARCHAR")]
        public string className { get; set; }

        [ForeignKey("Course")]
        public Guid courseId { get; set; }

        [ForeignKey("Mentor")]
        public Guid mentorId { get; set; }

        public virtual Course course { get; set; }
        public virtual MentorDetails mentor { get; set; }
        public virtual ICollection<Post> posts { get; set; }
        public virtual ICollection<Quiz> quizzes { get; set; }
        public virtual ICollection<StudentClass> studentClasses { get; set; }
    }
}

