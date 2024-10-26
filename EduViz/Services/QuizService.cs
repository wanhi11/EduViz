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
using EduViz.Common.Payloads.Request;
using EduViz.Common.Payloads.Response;
using EduViz.Exceptions;
using Microsoft.VisualBasic;
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
    private readonly IRepository<MentorDetails, Guid> _mentorRepository;
    private readonly IRepository<Course, Guid> _courseRepository;
    private readonly IRepository<StudentQuizScore, Guid> _scoreRepository;
    private readonly IRepository<StudentAnswer, Guid> _quizHistoryRepository;


    public QuizService(IRepository<Quiz, Guid> quizRepository, IMapper mapper,
        IRepository<Question, Guid> questionRepository, IRepository<Class, Guid> classRepository,
        IRepository<StudentQuizScore, Guid> studentQuizScoreRepository,
        IRepository<MentorDetails, Guid> mentorRepository,
        IRepository<Course, Guid> courseRepository,
        IRepository<StudentQuizScore,Guid> scoreRepository,
        IRepository<StudentAnswer,Guid> quizHistoryRepository)
    {
        _mapper = mapper;
        _quizRepository = quizRepository;
        _questionRepository = questionRepository;
        _classRepository = classRepository;
        _studentQuizScoreRepository = studentQuizScoreRepository;
        _mentorRepository = mentorRepository;
        _courseRepository = courseRepository;
        _scoreRepository = scoreRepository;
        _quizHistoryRepository = quizHistoryRepository;
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

    public async Task<UploadQuestionsReponse> ImportQuestionsAsync(IFormFile file, Guid quizId)
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
                var existingQuestion = _questionRepository.FirstOrDefault(q =>
                    q.questionText == parsedQuestion.QuestionText
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
                        QuestionText = Regex.Replace(text,@"^Question \d+:\s*","").Trim(),
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

    public async Task<GetAllQuizByCourseResponse> GetAllQuizzesByMentor(Guid mentorId)
    {
        // Lấy thông tin mentor
        var mentorDetails = await _mentorRepository
            .FindByCondition(m => m.mentorDetailsId == mentorId)
            .Include(m => m.courses)
                .ThenInclude(c => c.classes)
                .ThenInclude(cl => cl.quizzes)
                .ThenInclude(q => q.questions)
            .Include(c => c.classes)
            .ThenInclude(cl => cl.studentClasses)
            .FirstOrDefaultAsync();

        if (mentorDetails == null)
        {
            throw new BadRequestException("Mentor not found.");
        }

        // Khởi tạo danh sách quiz
        var quizList = new List<QuizInCourse>();

        foreach (var course in mentorDetails.courses)
        {
            foreach (var classInfo in course.classes)
            {

                if (!classInfo.quizzes.Any())
                {
                    continue;
                }

                foreach (var quiz in classInfo.quizzes)
                {
                    int totalStudent = 0;
                    int numOfStuAttempt = 0;
                    if (classInfo.studentClasses.Any())
                    {
                        totalStudent = classInfo.studentClasses.Count(); // Tổng số học viên trong lớp    
                        numOfStuAttempt = await _studentQuizScoreRepository
                            .FindByCondition(sqs => sqs.quizId == quiz.quizId)
                            .Select(sqs => sqs.userId)
                            .Distinct()
                            .CountAsync(); // Số lượng học viên đã làm quiz
                    }

                    var quizInClass = new QuizInCourse
                    {
                        quizId = quiz.quizId,
                        quizTitle = quiz.quizTitle,
                        duration = quiz.duration.ToString(@"hh\:mm\:ss"),
                        totalStudent = totalStudent,
                        numOfStuAttempt = numOfStuAttempt,
                        numOfQuestion = quiz.questions.Count() // Số lượng câu hỏi trong quiz
                    };

                    quizList.Add(quizInClass);
                }
            }
        }

        return new GetAllQuizByCourseResponse()
        {
            quizzes = quizList
        };
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
                quizId = quiz.quizId,
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

    public async Task<List<QuestionModel>> GetQuestionsByQuizId(Guid quizId)
    {
        var questions = await _questionRepository.FindByCondition(q => q.quizId.Equals(quizId)).ToListAsync();
        if (!questions.Any())
        {
            throw new BadRequestException("This Quiz do not have any question yet");
        }

        return _mapper.Map<List<QuestionModel>>(questions);
    }

    public async Task<QuizModel?> GetQuizByQuizId(Guid quizId)
    {
        var quiz = await _quizRepository.FindByCondition(q => q.quizId.Equals(quizId)).FirstOrDefaultAsync();
        if (quiz is null) return null; 
        return _mapper.Map<QuizModel>(quiz);
    }

    public async Task<double> CalScoreStudent(List<QuestionWithAnswer> qaaList, Guid quizId,Guid scoreHistoryId)
    {
        int totalScore = 0;
        List<StudentAnswer> listChosenAnswer = new List<StudentAnswer>();
        var questions  = await _questionRepository.FindByCondition(q => q.quizId.Equals(quizId)).ToListAsync();
        if (!questions.Any())
        {
            throw new BadRequestException("Quiz has no questions");
        }

        foreach (var qaa in qaaList)
        {
            var question = await _questionRepository.FindByCondition(q => q.questionId.Equals(qaa.questionId))
                .FirstAsync();
            var historyEntity = new StudentAnswer()
            {
                selectedAnswer = qaa.chosenAnswer,
                quizId = quizId,
                questionId = qaa.questionId,
                studentAnswerId = Guid.NewGuid(),
                studentQuizScoreId = scoreHistoryId
            };
            listChosenAnswer.Add(historyEntity);
            if (qaa.chosenAnswer.Equals(question.correctAnswer))
            {
                totalScore++;
            }
        }

        await _quizHistoryRepository.AddRangeAsync(listChosenAnswer);
        
        return Math.Round(10*((double)totalScore/questions.Count),2);
    }

    public async Task<StudentQuizScoreModel> SaveStudentScore(StudentQuizScoreModel req)
    {
       
       var result = await _studentQuizScoreRepository.AddAsync(_mapper.Map<StudentQuizScore>(req));
       if (!(await _studentQuizScoreRepository.Commit() > 0))
       {
           throw new BadRequestException("Can not add student quiz result");
       }
       return _mapper.Map<StudentQuizScoreModel>(result);
    }

    public async Task<StudentQuizScoreModel> UpdateScore(StudentQuizScoreModel req)
    {
        var existingScore = await _studentQuizScoreRepository
            .FindByCondition(sqs => sqs.studentQuizScoreId == req.studentQuizScoreId)
            .FirstOrDefaultAsync();

        if (existingScore == null)
        {
            throw new NotFoundException("Student quiz score not found.");
        }

        existingScore.score = req.score; 

        _studentQuizScoreRepository.Update(existingScore);

        if (!(await _studentQuizScoreRepository.Commit() > 0))
        {
            throw new BadRequestException("Cannot update student quiz result");
        }

        return _mapper.Map<StudentQuizScoreModel>(existingScore);
    }
    

    public async Task<List<StudentQuizScoreModel>?> GetQuizHistoryWithExactCourse(Guid studentId, Guid courseId)
    {
        var result = await _studentQuizScoreRepository.FindByCondition(s => s.userId.Equals(studentId)&& s.quiz.mentorClass.classId.Equals(courseId))
            .Include(s => s.quiz)
            .ThenInclude(q => q.mentorClass)
            .ToListAsync();
        if (!result.Any()) return null;
        return _mapper.Map<List<StudentQuizScoreModel>>(result);
    }

    public async Task<List<StudentQuizScoreModel>?> GetAllQuizHistory(Guid studentId)
    {
        var result = await _studentQuizScoreRepository.FindByCondition(s =>
            s.userId.Equals(studentId)).ToListAsync();

        if (!result.Any()) return null;
        return _mapper.Map<List<StudentQuizScoreModel>>(result);
    }

    public async Task<List<StudentQuizScoreModel>?> GetQuizHistory(Guid studentId, Guid quizId)
    {
        var result = await _studentQuizScoreRepository.FindByCondition(s =>
            s.userId.Equals(studentId) && s.quizId.Equals(quizId)).ToListAsync();
        if (!result.Any()) return null;
        return _mapper.Map<List<StudentQuizScoreModel>>(result);

    }

    public async Task<List<QuestionWithStudentAnswer>> GetQuestionWithStudentAnswer(Guid studentId, Guid scoreId)
    {
        var result = await _quizHistoryRepository.FindByCondition(sa => sa.studentQuizScoreId.Equals(scoreId))
            .Include(sa => sa.studentQuizScore)
            .Include(sa => sa.question)
            .Select(sa => new QuestionWithStudentAnswer
            {
                questionId = sa.questionId,
                questionText = sa.question.questionText,
                answerA = sa.question.answerA,
                answerB = sa.question.answerB,
                answerC = sa.question.answerC,
                answerD = sa.question.answerD,
                studentAnswer = sa.selectedAnswer,
                correctAnswer = sa.question.correctAnswer
            }).ToListAsync();
        if (!result.Any())
        {
            throw new BadRequestException("Student have not joined the test");
        }

        return result;
    }

    public QuizModel GetQuizModelFromQuizScore(Guid scoreId)
    {
        var result = _scoreRepository.FindByCondition(s =>
                s.studentQuizScoreId.Equals(scoreId))
            .Include(s => s.quiz)
            .First();
        return _mapper.Map<QuizModel>(result.quiz);
    }
    
    public StudentQuizScoreModel? GetScoreModelById (Guid scoreId)
    {
        var result = _scoreRepository.FindByCondition(x => x.studentQuizScoreId.Equals(scoreId)).FirstOrDefault();
        if (result is null) return null;
        return _mapper.Map<StudentQuizScoreModel>(result);

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