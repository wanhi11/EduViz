using EduViz.Dtos;
using EduViz.Helpers;

namespace EduViz.Common.Payloads.Request;

public class CreateUserRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public static class CreateUserRequestExtensions
{
    public static UserModel ToUserModel(this CreateUserRequest req)
    {
        var userModel = new UserModel()
        {
            UserId = Guid.NewGuid(),
            Email = req.Email,
            Password = SecurityUtil.Hash(req.Password)
        };
        return userModel;
    }
}