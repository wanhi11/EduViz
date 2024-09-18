using EduViz.Common.Payloads.Request;
using EduViz.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduViz.Controllers;

[ApiController]
[Route("api/[controller]")]
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
    public async Task<IActionResult> CreateMentor(CreateMentorRequest req)
    {
        return Ok();
    }
}