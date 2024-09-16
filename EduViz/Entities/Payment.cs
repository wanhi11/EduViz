using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EduViz.Enums;

namespace EduViz.Entities
{

    [Table("Payments")]
    public class Payment
    {
        [Key]
        public Guid PaymentId { get; set; }

        [ForeignKey("Student")]
        public Guid StudentId { get; set; }

        [ForeignKey("Mentor")]
        public Guid MentorId { get; set; }

        [ForeignKey("Course")]
        public Guid CourseId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }
        

        public PaymentStatus? PaymentStatus { get; set; }
        public virtual User Student { get; set; }
        public virtual MentorDetails Mentor { get; set; }
        public virtual Course Course { get; set; }
    }
}
