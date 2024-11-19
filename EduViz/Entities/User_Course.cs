using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace EduViz.Entities
{

    [Table("UserCourses")]
    public class UserCourse
    {
        [Key]
        public Guid userCourseId { get; set; }

        [ForeignKey("User")]
        public Guid userId { get; set; }

        [ForeignKey("Course")]
        public Guid courseId { get; set; }
        public string? comment { get; set; }
        public DateTime? commentDate { get; set; }
        public int? ratingStar { get; set; }

        public virtual User user { get; set; }
        public virtual Course course { get; set; }
    }
}
