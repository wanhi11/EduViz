using EduViz.Common.Payloads;
using EduViz.Common.Payloads.Response;
using EduViz.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace EduViz.Controllers;

[ApiController]
[Route("api/subject")]
public class SubjectController:ControllerBase
{
    private readonly SubjectService _subjectService;

    public SubjectController(SubjectService subjectService)
    {
        _subjectService = subjectService;
    }

    [HttpGet]
    public IActionResult GetAllSubject()
    {
        var result = _subjectService.GetAllSubject();
        return Ok(ApiResult<GetAllSubjectResponse>.Succeed(new GetAllSubjectResponse()
        {
            listSubject = result
        }));
    }
}