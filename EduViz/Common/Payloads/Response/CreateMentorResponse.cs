using EduViz.Enums;

namespace EduViz.Common.Payloads.Response;

public class CreateMentorResponse
{
    public string Name { get; set; }

    public string Email { get; set; }
    public List<string> Subject { get; set; }
    public string? Gender { get; set; }
}