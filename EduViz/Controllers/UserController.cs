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
            gender = result.Gender is null? null: result.Gender.ToString(),
            email = result.Email,
            name = result.UserName,
            subject = req.subject
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
            email = userModel.Email
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
                userId = user.UserId.ToString(),
                mentorId = mentor.MentorDetailsId.ToString(),
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
            userId = user.UserId.ToString(),
            email = user.Email,
            gender = user.Gender is null?null:user.Gender.ToString(),
            role = user.Role.ToString(),
            avatar = user.Avatar,
            name = user.UserName,
        }));
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult GetAllAccount()
    {
        var listResult = _userService.GetAllUser();
        var listResponse = new List<GetAccountModel>();
        foreach (var result in listResult)
        {
            listResponse.Add(new GetAccountModel()
            {
                Email = result.Email,
                Gender = result.Gender is null ? "": result.Gender.ToString() ,
                Role = result.Role.ToString(),
                Avatar = result.Avatar is null ? "": result.Avatar.ToString(),
                UserId = result.UserId,
                UserName = result.UserName
            });
        }
        return Ok(ApiResult<GetAllAccountResponse>.Succeed(new GetAllAccountResponse()
        {
            userList = listResponse
        }));
    }
    [HttpGet]
    [Route("{userId}")]
    [Authorize]
    public async Task<IActionResult> GetUserDetailsById(Guid userId)
    {
        
        var user = _userService.GetUserById(userId);

        if (user.Role.ToString().Equals("Mentor"))
        {
            var mentor = _mentorService.GetByMentorId(user.UserId);
            
            return Ok(ApiResult<UserDetailMentorResponse>.Succeed(new UserDetailMentorResponse()
            {
                userId = user.UserId.ToString(),
                mentorId = mentor.MentorDetailsId.ToString(),
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
            userId = user.UserId.ToString(),
            email = user.Email,
            gender = user.Gender is null?null:user.Gender.ToString(),
            role = user.Role.ToString(),
            avatar = user.Avatar,
            name = user.UserName,
        }));
    }

}