using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EduViz.Enums;

namespace EduViz.Entities;
[Table("UpgradeOrderDetails")]
public class UpgradeOrderDetails
{
    [Key]
    public Guid UpgradeOrderDetailsId { get; set; }
    
    public long OrderCode { get; set; }
    public int Amount { get; set; }
    public string PackageName { get; set; }

    [ForeignKey("MentorDetails")] 
    public Guid MentorDetailsID { get; set; }
    
    public PaymentStatus PaymentStatus { get; set; }
    
    public virtual MentorDetails MentorDetails { get; set; }
}