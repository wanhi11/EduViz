namespace EduViz.Common.Payloads.Request;

public class CoursePurchaseRequest
{
    public Guid courseId { get; set; }
    public string cancelUrl { get; set; } 
    public string returnUrl { get; set; }
}

