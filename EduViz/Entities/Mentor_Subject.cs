using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace EduViz.Entities
{

    [Table("MentorSubjects")]
    public class MentorSubject
    {
        [Key]
        public Guid mentorSubjectId { get; set; }

        [ForeignKey("Mentor")]
        public Guid mentorId { get; set; }

        [ForeignKey("Subject")]
        public Guid subjectId { get; set; }

        public virtual MentorDetails mentor { get; set; }
        public virtual Subject subject { get; set; }
    }
}
