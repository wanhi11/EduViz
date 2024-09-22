using EduViz.Enums;

namespace EduViz.Common.Payloads.Request;

public class CreateMentorRequest
{
    public string name { get; set; }

    public string email { get; set; }

    public string password { get; set; }
    public List<string> subject { get; set; }
    public Gender gender { get; set; }
}
