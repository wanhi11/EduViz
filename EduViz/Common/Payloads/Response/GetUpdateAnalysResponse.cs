namespace EduViz.Common.Payloads.Response;

public class GetUpdateAnalysisResponse
{
    public List<UpgradeAnalysis> analyses { get; set; }
}

public class UpgradeAnalysis
{
    public Guid upgradeOrderDetailsId { get; set; }
    
    public long orderCode { get; set; }
    
    public int amount { get; set; }
    
    public string packageName { get; set; }
    
    public DateTime UpgradeDate { get; set; }
    
    public string mentorName { get; set; }


}