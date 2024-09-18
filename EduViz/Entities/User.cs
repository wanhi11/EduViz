using EduViz.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EduViz.Entities
{

    [Table("Users")]
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public Role Role { get; set; }
        
        public string? Avatar { get; set; }

        public virtual MentorDetails MentorDetails { get; set; }
        public virtual ICollection<UserCourse> UserCourses { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<StudentClass> StudentClasses { get; set; }
        public virtual ICollection<StudentQuizScore> StudentQuizScores { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
