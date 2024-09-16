namespace EduViz.Common.Payloads.Request;

public class VipUpgradeRequest
{
    public Guid MentorDetailID { get; set; }
    public string CancelUrl { get; set; } 
    public string ReturnUrl { get; set; }
    public int MonthDuration { get; set; }
}
