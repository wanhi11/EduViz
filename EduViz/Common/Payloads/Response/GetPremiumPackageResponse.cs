namespace EduViz.Common.Payloads.Response;

public class GetPremiumPackageResponse
{
    public List<PremiumPackageInfo> premiumPackageInfos { get; set; }
}

public record PremiumPackageInfo
{
    public string packageName { get; set; }
    public int amout { get; set; }
    public int monthDuraion { get; set; }
}