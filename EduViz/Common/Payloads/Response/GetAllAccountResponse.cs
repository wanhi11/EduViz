using EduViz.Dtos;

namespace EduViz.Common.Payloads.Response;

public class GetAllAccountResponse
{
    public List<UserModel> userList { get; set; }
}