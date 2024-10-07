using EduViz.Common.Payloads;
using EduViz.Common.Payloads.Request;
using EduViz.Common.Payloads.Response;
using EduViz.Exceptions;
using EduViz.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduViz.Controllers;
[ApiController]
[Route("api/quiz")]
public class QuizController: ControllerBase
{
    private readonly QuizService _quizService;

    public QuizController(QuizService quizService)
    {
        _quizService = quizService;
    }

    [HttpPost("upload-questions")]
    [Authorize(Roles = "Mentor")]
    public async Task<IActionResult> UploadQuestions([FromForm] UploadQuestionsRequest file)
    {
        if (file.file == null || file.file.Length == 0)
        {
            throw new BadRequestException("No file provided.");
        }

        // Kiểm tra kiểu file
        var fileExtension = Path.GetExtension(file.file.FileName);
        if (fileExtension.Equals(".docx", StringComparison.OrdinalIgnoreCase) == false)
        {
            throw new BadRequestException("Invalid file type. Please upload a .docx file.");
        }
        var quiz = await _quizService.CreateQuiz(file.ToQuizModel());
        if (quiz is null)
        {
            throw new BadRequestException("Quiz has been created");
        }

        var result = await _quizService.ImportQuestionsAsync(file.file, quiz.quizId);
        result.duration = quiz.duration;
        result.quizId = quiz.quizId.ToString();
        result.quizTitle = quiz.quizTitle;

        return Ok(ApiResult<UploadQuestionsReponse>.Succeed(result));
    }
    //
    // [HttpPost("submit-quiz")]
    // [Authorize(Roles = "Student")]
    // public async Task<IActionResult> SubmitQuiz([FromBody] )
    
}