using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace EduViz.Entities
{

    [Table("UserCourses")]
    public class UserCourse
    {
        [Key]
        public Guid UserCourseId { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [ForeignKey("Course")]
        public Guid CourseId { get; set; }

        public virtual User User { get; set; }
        public virtual Course Course { get; set; }
    }
}
