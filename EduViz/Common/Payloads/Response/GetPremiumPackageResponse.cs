namespace EduViz.Common.Payloads.Response;

public class GetPremiumPackageResponse
{
    public List<PremiumPackageInfo> PremiumPackageInfos { get; set; }
}

public record PremiumPackageInfo
{
    public string PackageName { get; set; }
    public int Amout { get; set; }
    public int MonthDuraion { get; set; }
}