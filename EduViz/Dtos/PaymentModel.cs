using EduViz.Enums;

namespace EduViz.Dtos;

public class PaymentModel
{
    public Guid PaymentId { get; set; }
    
    public Guid StudentId { get; set; }
    
    public Guid MentorId { get; set; }
    
    public Guid CourseId { get; set; }
    public int Amount { get; set; }
             
    public DateTime PaymentDate { get; set; }
    
    public string TransactionId { get; set; }

    public PaymentStatus? PaymentStatus { get; set; }
}