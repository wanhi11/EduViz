using EduViz.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EduViz.Entities
{

    [Table("Users")]
    public class User
    {
        [Key]
        public Guid userId { get; set; }
        
        [MaxLength(100)]
        public string? userName { get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        public string password { get; set; }

        [Required]
        public Role role { get; set; }
        
        public string? avatar { get; set; }
        public Gender? gender { get; set; }

        public virtual MentorDetails mentorDetails { get; set; }
        public virtual ICollection<UserCourse> userCourses { get; set; }
        public virtual ICollection<Comment> comments { get; set; }
        public virtual ICollection<StudentClass> studentClasses { get; set; }
        public virtual ICollection<StudentQuizScore> studentQuizScores { get; set; }
        public virtual ICollection<Payment> payments { get; set; }
    }
}
