namespace EduViz.Common.Payloads.Request;

public class LoginRequest
{
    public string email { get; set; } = null!;
    public string password { get; set; } = null!;
}