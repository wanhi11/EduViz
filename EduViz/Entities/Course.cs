    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;
    using EduViz.Enums;

    namespace EduViz.Entities
    {

        [Table("Courses")]
        public class Course
        {
            [Key]
            public Guid CourseId { get; set; }

            [Required]
            [MaxLength(200)]
            public string CourseName { get; set; }

            [ForeignKey("Mentor")]
            public Guid MentorId { get; set; }

            [ForeignKey("Subject")]
            public Guid SubjectId { get; set; }

            [Required]
            public decimal Price { get; set; }
            
            public string? Picture { get; set; }
            
            public DateTime StartDate { get; set; }
            public int Duration { get; set; }
            
            public Schedule Schedule { get; set; }

            public virtual MentorDetails Mentor { get; set; }
            public virtual Subject Subject { get; set; }
            public virtual ICollection<Class> Classes { get; set; }
            public virtual ICollection<UserCourse> UserCourses { get; set; }
            public virtual ICollection<Payment> Payments { get; set; }
        }
    }
