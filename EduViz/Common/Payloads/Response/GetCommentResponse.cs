using DocumentFormat.OpenXml.Office2010.ExcelAc;

namespace EduViz.Common.Payloads.Response;

public class GetCommentResponse
{
    public List<FeedBack> feedBacks { get; set; }
}

public class FeedBack
{
    public string userName { get; set; }
    public int ratingStar { get; set; }
    public DateTime commentDate { get; set; }
    public string comment { get; set; }
}
