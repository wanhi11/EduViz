using AutoMapper;
using EduViz.Dtos;
using EduViz.Entities;
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
}