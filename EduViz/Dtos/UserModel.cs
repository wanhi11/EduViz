using System.ComponentModel.DataAnnotations;
using EduViz.Enums;

namespace EduViz.Dtos;

public class UserModel
{
    public Guid UserId { get; set; }

    [Required] [MaxLength(100)] public string UserName { get; set; }

    [Required] [EmailAddress] public string Email { get; set; }

    [Required] public string Password { get; set; }

    [Required] public Role Role { get; set; }
}