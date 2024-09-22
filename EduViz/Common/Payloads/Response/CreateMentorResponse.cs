using EduViz.Enums;

namespace EduViz.Common.Payloads.Response;

public class CreateMentorResponse
{
    public string name { get; set; }

    public string email { get; set; }
    public List<string> subject { get; set; }
    public string? gender { get; set; }
}