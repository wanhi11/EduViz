using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace EduViz.Entities
{

    [Table("StudentClasses")]
    public class StudentClass
    {
        [Key]
        public Guid studentClassId { get; set; }

        [ForeignKey("User")]
        public Guid userId { get; set; }

        [ForeignKey("Class")]
        public Guid classId { get; set; }

        public virtual User user { get; set; }
        public virtual Class mentorClass { get; set; }
    }
}
