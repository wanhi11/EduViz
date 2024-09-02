using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace EduViz.Entities
{

    [Table("Subjects")]
    public class Subject
    {
        [Key]
        public Guid SubjectId { get; set; }

        [Required]
        [MaxLength(150)]
        public string SubjectName { get; set; }

        public virtual ICollection<MentorSubject> MentorSubjects { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}
