using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EduViz.Dtos.Auth;
using EduViz.Entities;
using EduViz.Exceptions;
using EduViz.Helpers;
using EduViz.Repositories;
using EduViz.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EduViz.Services;

public class IdentityService
{
    private readonly JwtSettings _jwtSettings;
    private readonly IOptions<JwtSettings> _jwtSettingsOptions;
    private readonly IRepository<User, Guid> _userRepository;
    private readonly IRepository<MentorDetails, Guid> _mentorRepository;

    public IdentityService(IOptions<JwtSettings> jwtSettingsOptions, IRepository<User,Guid>userRepository,
        IRepository<MentorDetails,Guid> mentorRepository)
    {
        _jwtSettings = jwtSettingsOptions.Value;
        _userRepository = userRepository;
        _mentorRepository = mentorRepository;
    }

    public LoginResult Login(string email, string password)
        {
            var user = _userRepository.FindByCondition(u => u.email == email).FirstOrDefault();
    
    
            if (user is null)
            {
                return new LoginResult
                {
                    Authenticated = false,
                    Token = null,
                };
            }
    
            var hash = SecurityUtil.Hash(password);
            if (!user.password.Equals(hash))
            {
                return new LoginResult
                {
                    Authenticated = false,
                    Token = null,
                };
            }
    
            return new LoginResult
            {
                Authenticated = true,
                Token = CreateJwtToken(user),
            };
        }
    
        private SecurityToken CreateJwtToken(User user)
        {
            var utcNow = DateTime.UtcNow;
          
            var authClaims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.NameId, user.userId.ToString()),
    /*            new(JwtRegisteredClaimNames.Sub, user.UserName),*/
                new(JwtRegisteredClaimNames.Email, user.email),
                new(ClaimTypes.Role, user.role.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            if (user.role.ToString().Equals("Mentor"))
            {
                string vipStatus = "IsVip";
                var mentor = _mentorRepository.FindByCondition(m => m.userId.Equals(user.userId))
                    .FirstOrDefault();
                if (mentor is null)
                {
                    throw new BadRequestException("Something went wrong");
                }
                else if(mentor.vipExpirationDate < DateTime.Now)
                {
                    vipStatus = "NotVip";
                }
                authClaims.Add(new Claim(ClaimTypes.UserData,vipStatus));
            }

            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
    
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(authClaims),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
                Expires = utcNow.Add(TimeSpan.FromDays(4)),
            };
    
            var handler = new JwtSecurityTokenHandler();
    
            var token = handler.CreateToken(tokenDescriptor);
    
            return token;
        }
}