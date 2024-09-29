using EduViz.Dtos;

namespace EduViz.Common.Payloads.Response;

public class GetAllAccountResponse
{
    public List<GetAccountModel> userList { get; set; }
}

public class GetAccountModel
{
    public Guid UserId { get; set; }
    
    public string? UserName { get; set; }
    
    public string Email { get; set; }
    public string Role { get; set; }
    public string? Gender { get; set; }
    public string? Avatar { get; set; }
}