using System.ComponentModel.DataAnnotations;
using EduViz.Enums;

namespace EduViz.Dtos;

public class UserModel
{
    public Guid UserId { get; set; }
    
    public string? UserName { get; set; }
    
    public string Email { get; set; }

    public string Password { get; set; }

    public string Role { get; set; }
    public string? Gender { get; set; }
    public string? Avatar { get; set; }
}