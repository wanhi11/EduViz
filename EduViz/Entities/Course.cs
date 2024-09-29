    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;
    using EduViz.Enums;

    namespace EduViz.Entities
    {

        [Table("Courses")]
        public class Course
        {
            [Key]
            public Guid courseId { get; set; }

            [Required]
            [MaxLength(200)]
            [Column(TypeName = "NVARCHAR")]
            public string courseName { get; set; }

            [ForeignKey("Mentor")]
            public Guid mentorId { get; set; }

            [ForeignKey("Subject")]
            public Guid subjectId { get; set; }

            [Required]
            public decimal price { get; set; }
            public string meetUrl { get; set; }
            
            public string? picture { get; set; }
            
            public DateTime startDate { get; set; }
            public int duration { get; set; }
            
            public Schedule schedule { get; set; }
            
            public TimeSpan beginingClass { get; set; }
            public TimeSpan endingClass { get; set; }

            public virtual MentorDetails mentor { get; set; }
            public virtual Subject subject { get; set; }
            public virtual ICollection<Class> classes { get; set; }
            public virtual ICollection<UserCourse> userCourses { get; set; }
            public virtual ICollection<Payment> payments { get; set; }
        }
    }
