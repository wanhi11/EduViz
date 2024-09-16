    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;
    namespace EduViz.Entities
    {

        [Table("MentorDetails")]
        public class MentorDetails
        {
            [Key]
            public Guid MentorDetailsId { get; set; }

            [ForeignKey("User")]
            public Guid UserId { get; set; }

            public DateTime VipExpirationDate { get; set; }

            public virtual User User { get; set; }
            public virtual ICollection<MentorSubject> MentorSubjects { get; set; }
            public virtual ICollection<Course> Courses { get; set; }
            public virtual ICollection<Class> Classes { get; set; }
            public virtual ICollection<Payment> Payments { get; set; }
            public virtual ICollection<UpgradeOrderDetails> UpdagradeOrderDetails { get; set; }
        }
    }
