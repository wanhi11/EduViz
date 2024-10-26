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
        var score = req.ToScoreModel();
        var addValue = await _quizService.SaveStudentScore(score);
        if (addValue is null)
        {
            throw new BadRequestException("Something went wrong");
        }
        var saveResult = await _quizService.CalScoreStudent(req.answerList,req.quizId,addValue.studentQuizScoreId);
        addValue.score = saveResult;

        await _quizService.UpdateScore(addValue);

        return Ok(ApiResult<QuizSubmitResponse>.Succeed(new QuizSubmitResponse()
        {
            score = saveResult,
            message = "Submit successfully"
        }));
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetQuizHistory([FromQuery] GetQuizHistoryWithExactCourseRequest req)
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

    [HttpGet("view-detail-history")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> QuizReview([FromBody] QuizReviewRequest req)
    {
        var result = await _quizService.GetQuestionWithStudentAnswer(req.studentId, req.scoreId);
        var quiz = _quizService.GetQuizModelFromQuizScore(req.scoreId);
        var score = _quizService.GetScoreModelById(req.scoreId);
        if (score is null) throw new BadRequestException("Can not have any score");
        return Ok(ApiResult<QuizReviewHistoryResponse>.Succeed(new QuizReviewHistoryResponse()
        {
            duration = score.duration,
            quizTitle = quiz.quizTitle,
            score = score.score,
            quizId = quiz.quizId,
            examDate = score.dateTaken,
            resultList = result
        }));
    }

    [HttpGet("{quizId:guid}/history/{userId:guid}")]
    public async Task<IActionResult> GetQuizHistory([FromRoute] Guid quizId, [FromRoute] Guid userId)
    {
        var result = await _quizService.GetQuizHistory(userId,quizId);
        if (result is null) throw new BadRequestException("Student have not taken any exams yet");
        return Ok(ApiResult<GetQuizHistoryWithExactCourseResponse>.Succeed(new GetQuizHistoryWithExactCourseResponse()
        {
            quizHistory = result
        }));
    }
}