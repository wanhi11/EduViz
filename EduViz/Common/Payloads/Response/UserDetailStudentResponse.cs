namespace EduViz.Common.Payloads.Response;

public class UserDetailStudentResponse
{
    public string? userId { get; set; }
    public string? name { get; set; }
    public string email { get; set; }
    public string role { get; set; }
        
    public string? avatar { get; set; }
    public string? gender { get; set; }
}