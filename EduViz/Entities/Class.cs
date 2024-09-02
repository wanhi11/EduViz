using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace EduViz.Entities
{
    [Table("Classes")]
    public class Class
    {
        [Key]
        public Guid ClassId { get; set; }

        [Required]
        [MaxLength(150)]
        public string ClassName { get; set; }

        [ForeignKey("Course")]
        public Guid CourseId { get; set; }

        [ForeignKey("Mentor")]
        public Guid MentorId { get; set; }

        public virtual Course Course { get; set; }
        public virtual MentorDetails Mentor { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Quiz> Quizzes { get; set; }
        public virtual ICollection<StudentClass> StudentClasses { get; set; }
    }
}

