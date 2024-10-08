using System.Text.RegularExpressions;
using AutoMapper;
using CloudinaryDotNet.Actions;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Packaging;
using EduViz.Dtos;
using EduViz.Entities;
using EduViz.Repositories;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.Wordprocessing;
using EduViz.Common.Payloads.Response;
using EduViz.Exceptions;
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using QuizModel = EduViz.Dtos.QuizModel;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;

namespace EduViz.Services;

public class QuizService
{
    private readonly IMapper _mapper;
    private readonly IRepository<Quiz, Guid> _quizRepository;
    private readonly IRepository<Question, Guid> _questionRepository;
    private readonly IRepository<Class, Guid> _classRepository;
    private readonly IRepository<StudentQuizScore, Guid> _studentQuizScoreRepository;

    public QuizService(IRepository<Quiz, Guid> quizRepository, IMapper mapper,
        IRepository<Question, Guid> questionRepository, IRepository<Class, Guid>classRepository,
        IRepository<StudentQuizScore,Guid> studentQuizScoreRepository)
    {
        _mapper = mapper;
        _quizRepository = quizRepository;
        _questionRepository = questionRepository;
        _classRepository = classRepository;
        _studentQuizScoreRepository = studentQuizScoreRepository;
    }

    public async Task<QuizModel?> CreateQuiz(QuizModel quiz)
    {
        var existClass = _classRepository.FindByCondition(c => c.classId.Equals(quiz.classId)).FirstOrDefault();
        if (existClass is null)
        {
            throw new BadRequestException("There is no suitable class");
        }

        var existedQuiz = _quizRepository.FindByCondition(q => q.quizTitle.Equals(quiz.quizTitle))
            .FirstOrDefault();
        if (existedQuiz is not null) return null;
        var quizEntity = _mapper.Map<Quiz>(quiz);
        await _quizRepository.AddAsync(quizEntity);
        if (!(await _quizRepository.Commit() > 0))
        {
            throw new BadRequestException("something went wrong went create quiz");
        }

        return _mapper.Map<QuizModel>(quizEntity);
    }

    public async Task<UploadQuestionsReponse> ImportQuestionsAsync(IFormFile file,Guid quizId)
    {
        var questions = new List<Question>();
        var duplicateCount = 0;

        using (var stream = file.OpenReadStream())
        {
            // Parsing logic using a library like OpenXML
            var parsedQuestions = ParseDocx(stream);

            foreach (var parsedQuestion in parsedQuestions)
            {
                // Check if the question already exists in the DB
                var existingQuestion = _questionRepository.FirstOrDefault(q => q.questionText == parsedQuestion.QuestionText 
                                                                               && q.quizId.Equals(quizId));
                if (existingQuestion != null)
                {
                    duplicateCount++;
                    continue;
                }

                var questionEntity = new Question
                {
                    questionId = Guid.NewGuid(),
                    questionText = parsedQuestion.QuestionText,
                    answerA = parsedQuestion.AnswerA,
                    answerB = parsedQuestion.AnswerB,
                    answerC = parsedQuestion.AnswerC,
                    answerD = parsedQuestion.AnswerD,
                    correctAnswer = parsedQuestion.CorrectAnswer,
                    picture = parsedQuestion.Picture,
                    quizId = quizId
                };

                questions.Add(questionEntity);
            }

            await _questionRepository.AddRangeAsync(questions);
        }

        var result = new UploadQuestionsReponse()
        {
            savedQuestions = _mapper.Map<List<QuestionModel>>(questions),
            duplicationCount = duplicateCount
        };

        return result;
    }

    private List<ParsedQuestion> ParseDocx(Stream fileStream)
    {
        var parsedQuestions = new List<ParsedQuestion>();

        using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(fileStream, false))
        {
            var body = wordDoc.MainDocumentPart.Document.Body;

            ParsedQuestion currentQuestion = null;

            foreach (var element in body.Elements<Paragraph>())
            {
                var text = element.InnerText.Trim();

                // Detect if it's a question
                if (Regex.IsMatch(text, @"^Question \d+:"))
                {
                    if (currentQuestion != null)
                    {
                        // Add the previously parsed question to the list
                        parsedQuestions.Add(currentQuestion);
                    }

                    // Start a new question
                    currentQuestion = new ParsedQuestion
                    {
                        QuestionText = text.Replace("Question ", "").Trim(),
                        AnswerA = string.Empty,
                        AnswerB = string.Empty,
                        AnswerC = string.Empty,
                        AnswerD = string.Empty,
                        CorrectAnswer = string.Empty
                    };
                }
                // Detect answers
                else if (text.StartsWith("A."))
                {
                    currentQuestion.AnswerA = text.Replace("A.", "").Trim();
                }
                else if (text.StartsWith("B."))
                {
                    currentQuestion.AnswerB = text.Replace("B.", "").Trim();
                }
                else if (text.StartsWith("C."))
                {
                    currentQuestion.AnswerC = text.Replace("C.", "").Trim();
                }
                else if (text.StartsWith("D."))
                {
                    currentQuestion.AnswerD = text.Replace("D.", "").Trim();
                }
                // Detect correct answer
                else if (text.StartsWith("Correct Answer:"))
                {
                    currentQuestion.CorrectAnswer = text.Replace("Correct Answer:", "").Trim();
                }
                // Detect images
                else if (element.Descendants<Drawing>().Any())
                {
                    var image = ExtractImageFromParagraph(element, wordDoc);
                    currentQuestion.Picture = image;
                }
            }

            // Add the last question
            if (currentQuestion != null)
            {
                parsedQuestions.Add(currentQuestion);
            }
        }

        return parsedQuestions;
    }

    // Helper function to extract image bytes from a paragraph
    private byte[] ExtractImageFromParagraph(Paragraph paragraph, WordprocessingDocument wordDoc)
    {
        var drawing = paragraph.Descendants<Drawing>().FirstOrDefault();
        if (drawing == null) return null;

        var blip = drawing.Descendants<DocumentFormat.OpenXml.Drawing.Blip>().FirstOrDefault();
        if (blip == null) return null;

        var imagePart = wordDoc.MainDocumentPart.GetPartById(blip.Embed.Value) as ImagePart;
        using (var stream = imagePart.GetStream())
        {
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
    public async Task<GetAllQuizByCourseResponse> GetAllQuizzesByCourse(Guid classId)
    {
        // Lấy thông tin lớp học
        var classInfo = await _classRepository.FindByCondition(c => c.classId == classId)
            .Include(c => c.quizzes)
            .ThenInclude(q => q.questions)
            .Include(c => c.studentClasses) // Lấy thông tin học viên của lớp
            .FirstOrDefaultAsync();

        if (classInfo == null)
        {
            throw new BadRequestException("Class not found.");
        }

        // Khởi tạo danh sách QuizInClass
        var quizList = new List<QuizInCourse>();

        foreach (var quiz in classInfo.quizzes)
        {
            var totalStudent = classInfo.studentClasses.Count(); // Tổng số học viên trong lớp

            var numOfStuAttempt = await _studentQuizScoreRepository.FindByCondition(sqs => sqs.quizId == quiz.quizId)
                .Select(sqs => sqs.userId)
                .Distinct()
                .CountAsync(); // Số lượng học viên đã làm quiz

            var quizInClass = new QuizInCourse
            {
                quizTitle = quiz.quizTitle,
                duration = quiz.duration.ToString(@"hh\:mm\:ss"), // Chuyển đổi thời gian
                totalStudent = totalStudent,
                numOfStuAttempt = numOfStuAttempt,
                numOfQuestion = quiz.questions.Count() // Số lượng câu hỏi trong quiz
            };

            quizList.Add(quizInClass);
        }

        return new GetAllQuizByCourseResponse
        {
            quizzes = quizList
        };
    }
}
public class ParsedQuestion
{
    public string QuestionText { get; set; }
    public string AnswerA { get; set; }
    public string AnswerB { get; set; }
    public string? AnswerC { get; set; } = string.Empty;
    public string? AnswerD { get; set; } = string.Empty;
    public string CorrectAnswer { get; set; }
    public byte[]? Picture { get; set; }
}
