using EduViz.Dtos;
using EduViz.Helpers;

namespace EduViz.Common.Payloads.Request;

public class CreateUserRequest
{
    public string name { get; set; }
    public string email { get; set; }
    public string password { get; set; }
}

public static class CreateUserRequestExtensions
{
    public static UserModel ToUserModel(this CreateUserRequest req)
    {
        var userModel = new UserModel()
        {
            UserId = Guid.NewGuid(),
            Email = req.email,
            Password = SecurityUtil.Hash(req.password),
            UserName = req.name
        };
        return userModel;
    }
}