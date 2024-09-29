using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EduViz.Enums;

namespace EduViz.Entities;
[Table("UpgradeOrderDetails")]
public class UpgradeOrderDetails
{
    [Key]
    public Guid upgradeOrderDetailsId { get; set; }
    
    public long orderCode { get; set; }
    public int amount { get; set; }
    public string packageName { get; set; }
    public DateTime UpgradeDate { get; set; }

    [ForeignKey("MentorDetails")] 
    public Guid mentorDetailsID { get; set; }
    
    public PaymentStatus paymentStatus { get; set; }
    
    public virtual MentorDetails mentorDetails { get; set; }
}