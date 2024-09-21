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
            Gender = result.Gender,
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
}