using EduViz.Common.Payloads;
using EduViz.Common.Payloads.Request;
using EduViz.Common.Payloads.Response;
using EduViz.Exceptions;
using EduViz.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduViz.Controllers;

[ApiController]
[Route("api/user")]
public class UserController:ControllerBase
{
    private readonly UserService _userService;
    private readonly MentorDetailService _mentorService;

    public UserController(UserService userService, MentorDetailService mentorService)
    {
        _userService = userService;
        _mentorService = mentorService;
    }

    [HttpPost]
    [Route("create-mentor")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateMentor([FromBody]CreateMentorRequest req)
    {
        var result = await _userService.CreateMentor(req);
        if (result is null)
        {
            throw new BadRequestException("Something went wrong");
        }
        return Ok(ApiResult<CreateMentorResponse>.Succeed(new CreateMentorResponse()
        {
            Gender = result.Gender is null? null: result.Gender.ToString(),
            Email = result.Email,
            Name = result.UserName,
            Subject = req.Subject
        }));
    }

    [HttpPost]
    [Route("sign-up")]
    public async Task<IActionResult> SignUp([FromBody] CreateUserRequest req)
    {
        var userModel = req.ToUserModel();
        var result = await _userService.CreateStudent(userModel);
        return Ok(ApiResult<CreateStudentResponse>.Succeed(new CreateStudentResponse()
        {
            Email = userModel.Email
        }));
    }

    [HttpGet]
    [Route("my-account")]
    [Authorize]
    public async Task<IActionResult> GetUserDetail()
    {
        
        Request.Headers.TryGetValue("Authorization", out var token);
        token = token.ToString().Split()[1];
        var user = _userService.GetUserInToken(token);

        if (user.Role.ToString().Equals("Mentor"))
        {
            var mentor = _mentorService.GetByMentorId(user.UserId);
            
            return Ok(ApiResult<UserDetailMentorResponse>.Succeed(new UserDetailMentorResponse()
            {
                email = user.Email,
                gender = user.Gender is null? null :user.Gender.ToString(),
                role = user.Role.ToString(),
                avatar = user.Avatar,
                name = user.UserName,
                expiredDate = mentor.VipExpirationDate,
                isVip = mentor.VipExpirationDate > DateTime.Now ? true : false  
            }));
        }

        return Ok(ApiResult<UserDetailStudentResponse>.Succeed(new UserDetailStudentResponse()
        {
            email = user.Email,
            gender = user.Gender is null?null:user.Gender.ToString(),
            role = user.Role.ToString(),
            avatar = user.Avatar,
            name = user.UserName,
        }));
    }
}