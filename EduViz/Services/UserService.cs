using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using EduViz.Common.Payloads.Request;
using EduViz.Dtos;
using EduViz.Entities;
using EduViz.Enums;
using EduViz.Exceptions;
using EduViz.Helpers;
using EduViz.Repositories;

namespace EduViz.Services;

public class UserService
{
    private readonly IRepository<User, Guid> _userRepository;
    private readonly IMapper _mapper;
    private readonly IRepository<Subject, Guid> _subjecRepository;
    private readonly IRepository<MentorDetails, Guid> _mentorRepository;

    public UserService(IRepository<User,Guid> userRepository,IMapper mapper,
        IRepository<Subject,Guid> subjecRepository,IRepository<MentorDetails,Guid> mentorRepository)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _subjecRepository = subjecRepository;
        _mentorRepository = mentorRepository;
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

    public async Task<UserModel?> CreateMentor(CreateMentorRequest mentor)
    {
        var existedMentor = _userRepository.FindByCondition(u => u.Email.Equals(mentor.Email)).FirstOrDefault();
        if (existedMentor is not null)
        {
            throw new BadRequestException("This Email has been used");
        }

        var user = new User()
        {
            UserName = mentor.Name,
            Gender = mentor.Gender,
            Email = mentor.Email,
            Password = SecurityUtil.Hash(mentor.Password),
            Role = Role.Mentor,
            UserId = Guid.NewGuid()
        };
        var mentorDetail = new MentorDetails()
        {
            MentorDetailsId = Guid.NewGuid(),
            UserId = user.UserId,
            VipExpirationDate = DateTime.Now
        };
        foreach (var subjectName in mentor.Subject)
        {
            var subject = _subjecRepository.FindByCondition(s => s.SubjectName.Equals(subjectName)).FirstOrDefault();
            if (subject is null)
            {
                throw new BadRequestException("Subject does not exist");
            }

            subject.MentorSubjects.Add(new MentorSubject()
            {
                MentorId = mentorDetail.MentorDetailsId,
                SubjectId = subject.SubjectId
            });
        }
        await _userRepository.AddAsync(user);
        await _mentorRepository.AddAsync(mentorDetail);
        var userResult = await _userRepository.Commit();
        if (userResult > 0)
        {
            return _mapper.Map<UserModel>(user);
        }

        if (!(await _mentorRepository.Commit() > 0))
        {
            throw new BadRequestException("something went wrong when create mentordetails");
        }

        return null;
    }

    public async Task<UserModel> CreateStudent(UserModel newStudent)
    {
        var user = _mapper.Map<User>(newStudent);
        var existUser = _userRepository.FindByCondition(u => u.Email.Equals(newStudent.Email)).FirstOrDefault();
        if (existUser is not null)
        {
            throw new BadRequestException("This Email has been used");
        }

        user.Role = Role.Student;
        var result = await _userRepository.AddAsync(user);
        if (result is null)
        {
            throw new BadRequestException("Something went wrong when create account");
        }

        return _mapper.Map<UserModel>(result);
    }
    
}