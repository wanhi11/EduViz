using EduViz.Enums;

namespace EduViz.Common.Payloads.Request;

public class CreateMentorRequest
{
    public string Name { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }
    public List<string> Subject { get; set; }
    public Gender Gender { get; set; }
}