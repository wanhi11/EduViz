using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using EduViz.Common.Payloads.Request;
using EduViz.Dtos;
using EduViz.Entities;
using EduViz.Enums;
using EduViz.Exceptions;
using EduViz.Helpers;
using EduViz.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EduViz.Services;

public class UserService
{
    private readonly IRepository<User, Guid> _userRepository;
    private readonly IMapper _mapper;
    private readonly IRepository<Subject, Guid> _subjecRepository;
    private readonly IRepository<MentorDetails, Guid> _mentorRepository;
    private readonly IRepository<Payment, Guid> _paymentRepository;
    private readonly IRepository<UserCourse, Guid> _userCourseRepository;

    public UserService(IRepository<User,Guid> userRepository,IMapper mapper,
        IRepository<Subject,Guid> subjecRepository,IRepository<MentorDetails,Guid> mentorRepository,
        IRepository<Payment, Guid> paymentRepository, IRepository<UserCourse,Guid> userCourseRepository)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _subjecRepository = subjecRepository;
        _mentorRepository = mentorRepository;
        _paymentRepository = paymentRepository;
        _userCourseRepository = userCourseRepository;
    }

    public async Task<UserModel> GetUserByEmail(string email)
    {
        return _mapper.Map<UserModel>(_userRepository.FindByCondition(u => u.email.Equals(email)).FirstOrDefault());
    }

    public UserModel GetUserById(Guid Id)
    {
        var result = _userRepository.FindByCondition(u => u.userId.Equals(Id)).FirstOrDefault();
        if (result is null)
        {
            throw new BadRequestException("There is no user");
        }

        return _mapper.Map<UserModel>(result);
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

        var user =  _userRepository.FindByCondition(x => x.email == email).FirstOrDefault();
        if (user is null)
        {
            throw new BadRequestException("Can not found User");
        }
        return _mapper.Map<UserModel>(user);
    }

    public async Task<UserModel?> CreateMentor(CreateMentorRequest mentor)
    {
        var existedMentor = _userRepository.FindByCondition(u => u.email.Equals(mentor.email)).FirstOrDefault();
        if (existedMentor is not null)
        {
            throw new BadRequestException("This Email has been used");
        }

        var user = new User()
        {
            userName = mentor.name,
            gender = mentor.gender,
            email = mentor.email,
            password = SecurityUtil.Hash(mentor.password),
            role = Role.Mentor,
            userId = Guid.NewGuid()
        };
        var mentorDetail = new MentorDetails()
        {
            mentorDetailsId = Guid.NewGuid(),
            userId = user.userId,
            vipExpirationDate = DateTime.Now.AddDays(7)
        };
        foreach (var subjectName in mentor.subject)
        {
            var subject = _subjecRepository.FindByCondition(s => s.subjectName.Equals(subjectName)).FirstOrDefault();
            if (subject is null)
            {
                throw new BadRequestException("Subject does not exist");
            }

            subject.mentorSubjects.Add(new MentorSubject()
            {
                mentorId = mentorDetail.mentorDetailsId,
                subjectId = subject.subjectId
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
        var existUser = _userRepository.FindByCondition(u => u.email.Equals(newStudent.Email)).FirstOrDefault();
        if (existUser != null)
        {
            throw new BadRequestException("This Email has been used");
        }

        user.role = Role.Student;
        var createdUser = await _userRepository.AddAsync(user);
        
        if (!(await _userRepository.Commit()>0))
        {
            throw new BadRequestException("Something went wrong when create account");
        }

        return _mapper.Map<UserModel>(createdUser);
    }

    public List<UserModel> GetUserByName(string name)
    {
        var users = _userRepository.FindByCondition(u => u.userName.Contains(name)).ToList();
        if (!users.Any())
        {
            throw new BadRequestException("There is no matching record");
        }

        return _mapper.Map<List<UserModel>>(users);
    }

    public async Task<bool> AddStudentToClass(Guid paymentId)
    {
        var payment = _paymentRepository.FindByCondition(p => p.paymentId.Equals(paymentId)).FirstOrDefault();
        if (payment is null)
        {
            throw new BadRequestException("there is no payment match");
        }

        var userCourse = new UserCourse()
        {
            userCourseId = Guid.NewGuid(),
            userId = payment.studentId,
            courseId = payment.courseId
        };
        await _userCourseRepository.AddAsync(userCourse);
        return await _userRepository.Commit() > 0;

    }

    public async Task<UserModel> GetStudentInCourse(Guid courseId)
    {
        var student = _userRepository.GetAll()
            .Include(u => u.studentQuizScores)
            .Include(u => u.userCourses)
            .ThenInclude(c => c.courseId.Equals(courseId))
            .ToList();
        
    }

}