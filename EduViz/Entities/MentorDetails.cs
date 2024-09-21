    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;
    namespace EduViz.Entities
    {

        [Table("MentorDetails")]
        public class MentorDetails
        {
            [Key]
            public Guid mentorDetailsId { get; set; }

            [ForeignKey("User")]
            public Guid userId { get; set; }

            public DateTime vipExpirationDate { get; set; }

            public virtual User user { get; set; }
            public virtual ICollection<MentorSubject> mentorSubjects { get; set; }
            public virtual ICollection<Course> courses { get; set; }
            public virtual ICollection<Class> classes { get; set; }
            public virtual ICollection<Payment> payments { get; set; }
            public virtual ICollection<UpgradeOrderDetails> upgradeOrderDetails { get; set; }
        }
    }
