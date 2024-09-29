namespace EduViz.Common.Payloads.Response;

public class UserDetailMentorResponse
{
    public string userId { get; set; }
    public string mentorId { get; set; }
    public string? name { get; set; }
    public string email { get; set; }
    public string role { get; set; }
        
    public string? avatar { get; set; }
    public string? gender { get; set; }
    public bool isVip { get; set;}
    public DateTime expiredDate { get; set; }
}