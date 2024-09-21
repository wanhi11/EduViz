using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace EduViz.Entities
{

    [Table("Subjects")]
    public class Subject
    {
        [Key]
        public Guid subjectId { get; set; }

        [Required]
        [MaxLength(150)]
        public string subjectName { get; set; }

        public virtual ICollection<MentorSubject> mentorSubjects { get; set; } = new HashSet<MentorSubject>();
        public virtual ICollection<Course> courses { get; set; }
    }
}
