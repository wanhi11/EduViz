using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace EduViz.Entities
{

    [Table("StudentClasses")]
    public class StudentClass
    {
        [Key]
        public Guid StudentClassId { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [ForeignKey("Class")]
        public Guid ClassId { get; set; }

        public virtual User User { get; set; }
        public virtual Class Class { get; set; }
    }
}
