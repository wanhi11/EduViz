using EduViz.Enums;

namespace EduViz.Dtos;

public class UpgradeOrderDetailModel
{
    public Guid UpgradeOrderDetailsId { get; set; }
    
    public long OrderCode { get; set; }
    public int Amount { get; set; }
    public string PackageName { get; set; }
    public DateTime UpgradeDate { get; set; }
    
    public Guid MentorDetailsID { get; set; }
    
    public PaymentStatus PaymentStatus { get; set; }
}