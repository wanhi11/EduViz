using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EduViz.Enums;

namespace EduViz.Entities
{

    [Table("Payments")]
    public class Payment
    {
        [Key]
        public Guid paymentId { get; set; }

        [ForeignKey("Student")]
        public Guid studentId { get; set; }

        [ForeignKey("Mentor")]
        public Guid mentorId { get; set; }

        [ForeignKey("Course")]
        public Guid courseId { get; set; }

        [Required]
        public decimal amount { get; set; }

        [Required]
        public DateTime paymentDate { get; set; }
        

        public PaymentStatus? paymentStatus { get; set; }
        public virtual User student { get; set; }
        public virtual MentorDetails mentor { get; set; }
        public virtual Course course { get; set; }
    }
}
