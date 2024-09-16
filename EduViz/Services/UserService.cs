using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using EduViz.Dtos;
using EduViz.Entities;
using EduViz.Exceptions;
using EduViz.Repositories;

namespace EduViz.Services;

public class UserService
{
    private readonly IRepository<User, Guid> _userRepository;
    private readonly IMapper _mapper;

    public UserService(IRepository<User,Guid> userRepository,IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserModel> GetUserByEmail(string email)
    {
        return _mapper.Map<UserModel>(_userRepository.FindByCondition(u => u.Email.Equals(email)).FirstOrDefault());
    }

    public async Task<UserModel> GetUserById(Guid Id)
    {
        return _mapper.Map<UserModel>(_userRepository.FindByCondition(u => u.UserId.Equals(Id)).FirstOrDefault());
    }
    public  UserModel GetUserInToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new BadRequestException("Authorization header is missing or invalid.");
        }
        // Decode the JWT token
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        // Check if the token is expired
        if (jwtToken.ValidTo < DateTime.UtcNow)
        {
            throw new BadRequestException("Token has expired.");
        }

        string email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;

        var user =  _userRepository.FindByCondition(x => x.Email == email).FirstOrDefault();
        if (user is null)
        {
            throw new BadRequestException("Can not found User");
        }
        return _mapper.Map<UserModel>(user);
    }
}