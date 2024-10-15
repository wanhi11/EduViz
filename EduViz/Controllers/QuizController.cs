using EduViz.Common.Payloads;
using EduViz.Common.Payloads.Request;
using EduViz.Common.Payloads.Response;
using EduViz.Dtos;
using EduViz.Exceptions;
using EduViz.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduViz.Controllers;

[ApiController]
[Route("api/quiz")]
public class QuizController : ControllerBase
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

    [HttpGet("{quizId:guid}")]
    public async Task<IActionResult> GetQuizDetail([FromRoute] Guid quizId)
    {
        var questions = await _quizService.GetQuestionsByQuizId(quizId);
        var quiz = await _quizService.GetQuizByQuizId(quizId);
        if (quiz is null)
        {
            throw new BadRequestException("There is no quiz");
        }

        return Ok(ApiResult<QuizzDetailResponse>.Succeed(new QuizzDetailResponse()
        {
            duration = quiz.duration,
            quizTitle = quiz.quizTitle,
            quizId = quizId,
            questionList = questions
        }));
    }

    [HttpPost("submit-quiz")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> SubmitQuiz([FromBody] QuizSubmitRequest req)
    {
        var result = await _quizService.CalScoreStudent(req.answerList,req.quizId);
        var score = req.ToScoreModel();
        score.score = result;
        var addValue = await _quizService.SaveStudentScore(score);
        if (addValue is null)
        {
            throw new BadRequestException("Something went wrong");
        }

        return Ok(ApiResult<QuizSubmitResponse>.Succeed(new QuizSubmitResponse()
        {
            score = result,
            message = "Submit successfully"
        }));
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetQuizHistory([FromBody] GetQuizHistoryWithExactCourseRequest req)
    {
        var result = await _quizService.GetQuizHistoryWithExactCourse(req.studentId, req.courseId);
        if (result is null) throw new BadRequestException("Student have not taken any exams yet");
        return Ok(ApiResult<GetQuizHistoryWithExactCourseResponse>.Succeed(new GetQuizHistoryWithExactCourseResponse()
        {
            quizHistory = result
        }));
    }

    [HttpGet("history/{studentId:guid}")]
    public async Task<IActionResult> GetAllQuizHistory([FromRoute] Guid studentId)
    {
        
        var result = await _quizService.GetAllQuizHistory(studentId);
        if (result is null) throw new BadRequestException("Student have not taken any exams yet");
        return Ok(ApiResult<GetQuizHistoryWithExactCourseResponse>.Succeed(new GetQuizHistoryWithExactCourseResponse()
        {
            quizHistory = result
        }));
    }
}