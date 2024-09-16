using Microsoft.IdentityModel.Tokens;

namespace EduViz.Dtos.Auth;

public class LoginResult
{
    public bool Authenticated { get; set; }
    public SecurityToken? Token { get; set; }
}