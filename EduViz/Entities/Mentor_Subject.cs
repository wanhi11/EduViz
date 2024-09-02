using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace EduViz.Entities
{

    [Table("MentorSubjects")]
    public class MentorSubject
    {
        [Key]
        public Guid MentorSubjectId { get; set; }

        [ForeignKey("Mentor")]
        public Guid MentorId { get; set; }

        [ForeignKey("Subject")]
        public Guid SubjectId { get; set; }

        public virtual MentorDetails Mentor { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
