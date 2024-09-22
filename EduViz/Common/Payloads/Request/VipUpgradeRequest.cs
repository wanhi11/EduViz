namespace EduViz.Common.Payloads.Request;

public class VipUpgradeRequest
{
    public Guid mentorDetailID { get; set; }
    public string cancelUrl { get; set; } 
    public string returnUrl { get; set; }
    public int monthDuration { get; set; }
}
