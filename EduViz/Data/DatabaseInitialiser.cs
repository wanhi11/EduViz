using EduViz.Entities;
using EduViz.Enums;
using EduViz.Helpers;
using Microsoft.EntityFrameworkCore;

namespace EduViz.Data;

public interface IDataInitialiser
{
    Task InitialiseAsync();
    Task SeedAsync();
    Task TrySeedAsync();
}

public class DatabaseInitialiser : IDataInitialiser
{
    private readonly ApplicationDbContext _context;

    public DatabaseInitialiser(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            // Migration Database - Create database if it does not exist
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        if (_context.Users.Any() && _context.MentorDetails.Any() && _context.Subjects.Any()
            && _context.Courses.Any() && _context.Classes.Any())
        {
            return;
        }

        var mentor = new User()
        {
            userId = Guid.NewGuid(),
            email = "mentor1@gmail.com",
            userName = "Mentor",
            password = SecurityUtil.Hash("123456"),
            role = Role.Mentor,
        };
        var mentor2 = new User()
        {
            userId = Guid.NewGuid(),
            email = "mentor2@gmail.com",
            userName = "Mentor2",
            password = SecurityUtil.Hash("123456"),
            role = Role.Mentor,
        };
        var mentor3 = new User()
        {
            userId = Guid.NewGuid(),
            email = "mentor3@gmail.com",
            userName = "Mentor3",
            password = SecurityUtil.Hash("123456"),
            role = Role.Mentor,
        };
        var admin = new User()
        {
            userId = Guid.NewGuid(),
            email = "admin@gmail.com",
            userName = "Admin",
            password = SecurityUtil.Hash("123456"),
            role = Role.Admin,
        };
        var student = new User()
        {
            userId = Guid.NewGuid(),
            email = "student@gmail.com",
            userName = "Student",
            password = SecurityUtil.Hash("123456"),
            role = Role.Student,
        };
        var vipMentor = new User()
        {
            userId = Guid.NewGuid(),
            email = "vipmentor@gmail.com",
            userName = "VipMentor",
            password = SecurityUtil.Hash("123456"),
            role = Role.Mentor,
        };
        var vipMentor2 = new User()
        {
            userId = Guid.NewGuid(),
            email = "vipmentor2@gmail.com",
            userName = "VipMentor2",
            password = SecurityUtil.Hash("123456"),
            role = Role.Mentor,
        };
        
        List<User> users = new List<User>()
        {
            student,
            admin,
            mentor,
            vipMentor,
            mentor2,
            mentor3,
            vipMentor2
        };
        var mentorDetail2 = new MentorDetails(){
            userId = mentor2.userId,
            mentorDetailsId = Guid.NewGuid(),
            vipExpirationDate = DateTime.ParseExact("12/09/2024", "dd/MM/yyyy", null),
        };
        
        var mentorDetail3 = new MentorDetails(){
            userId = mentor3.userId,
            mentorDetailsId = Guid.NewGuid(),
            vipExpirationDate = DateTime.ParseExact("12/09/2024", "dd/MM/yyyy", null),
        };
        var normalMentorDetails = new MentorDetails()
        {
            userId = mentor.userId,
            mentorDetailsId = Guid.NewGuid(),
            vipExpirationDate = DateTime.ParseExact("12/09/2024", "dd/MM/yyyy", null),
        };
        var vipMentorDetails = new MentorDetails()
        {
            userId = vipMentor.userId,
            mentorDetailsId = Guid.NewGuid(),
            vipExpirationDate = DateTime.ParseExact("31/03/2025", "dd/MM/yyyy", null)
        };
        var vipMentorDetails2 = new MentorDetails()
        {
            userId = vipMentor2.userId,
            mentorDetailsId = Guid.NewGuid(),
            vipExpirationDate = DateTime.ParseExact("31/03/2025", "dd/MM/yyyy", null)
        };
        List<MentorDetails> mentorDetailList = new List<MentorDetails>()
        {
            normalMentorDetails,
            vipMentorDetails,
            vipMentorDetails2,mentorDetail3,mentorDetail2
        };
        var math = new Subject()
        {
            subjectId = Guid.NewGuid(),
            subjectName = "Toán"
        };
        var english = new Subject()
        {
            subjectId = Guid.NewGuid(),
            subjectName = "Tiếng Anh"
        };
        var hoa = new Subject()
        {
            subjectId = Guid.NewGuid(),
            subjectName = "Hóa"
        };
        var publicSpeaking = new Subject()
        {
            subjectId = Guid.NewGuid(),
            subjectName = "Public Speaking"
        }; 
        var hoihoa = new Subject()
        {
            subjectId = Guid.NewGuid(),
            subjectName = "Vẽ"
        };
        List<Subject> monhoc = new List<Subject>()
        {
            math,
            english,
            hoa,
            publicSpeaking,
            hoihoa
        };
        var course1 = new Course()
        {
            subjectId = math.subjectId,
            duration = 1,
            schedule = Schedule.SatSun,
            courseName = "Toán thầy A",
            startDate = DateTime.ParseExact("12/10/2024", "dd/MM/yyyy", null),
            price = 1000,
            courseId = Guid.NewGuid(),
            mentorId = normalMentorDetails.mentorDetailsId,
            beginingClass = new TimeSpan(15,30,00),
            endingClass = new TimeSpan(17,30,00)
        };
        var course2 = new Course()
        {
            subjectId = hoa.subjectId,
            duration = 2,
            schedule = Schedule.MonWedFri,
            courseName = "Hóa thầy A",
            startDate = DateTime.ParseExact("13/10/2024", "dd/MM/yyyy", null),
            price = 15000,
            courseId = Guid.NewGuid(),
            mentorId = normalMentorDetails.mentorDetailsId,
            beginingClass = new TimeSpan(19,30,00),
            endingClass = new TimeSpan(21,30,00)
        };
        var course3 = new Course()
        {
            subjectId = english.subjectId,
            duration = 3,
            schedule = Schedule.MonWedFri,
            courseName = "Tiếng anh thầy B",
            startDate = DateTime.ParseExact("14/10/2024", "dd/MM/yyyy", null),
            price = 20000,
            courseId = Guid.NewGuid(),
            mentorId = vipMentorDetails.mentorDetailsId,
            beginingClass = new TimeSpan(18,45,00),
            endingClass = new TimeSpan(20,45,00)
        };
        var course4 = new Course()
        {
            subjectId = publicSpeaking.subjectId,
            duration = 1,
            schedule = Schedule.SatSun,
            courseName = "Public Speaking cho trẻ",
            startDate = DateTime.ParseExact("12/10/2024", "dd/MM/yyyy", null),
            price = 1000,
            courseId = Guid.NewGuid(),
            mentorId = vipMentorDetails2.mentorDetailsId,
            beginingClass = new TimeSpan(15,30,00),
            endingClass = new TimeSpan(17,30,00)
        };
        var course5 = new Course()
        {
            subjectId = publicSpeaking.subjectId,
            duration = 1,
            schedule = Schedule.SatSun,
            courseName = "Public Speaking cho trẻ 2",
            startDate = DateTime.ParseExact("12/10/2024", "dd/MM/yyyy", null),
            price = 1000,
            courseId = Guid.NewGuid(),
            mentorId = mentorDetail2.mentorDetailsId,
            beginingClass = new TimeSpan(15,30,00),
            endingClass = new TimeSpan(17,30,00)
        };
        var course6 = new Course()
        {
            subjectId = publicSpeaking.subjectId,
            duration = 1,
            schedule = Schedule.SatSun,
            courseName = "Public Speaking cho trẻ 3",
            startDate = DateTime.ParseExact("12/10/2024", "dd/MM/yyyy", null),
            price = 1000,
            courseId = Guid.NewGuid(),
            mentorId = mentorDetail3.mentorDetailsId,
            beginingClass = new TimeSpan(15,30,00),
            endingClass = new TimeSpan(17,30,00)
        };
        var course7 = new Course()
        {
            subjectId = publicSpeaking.subjectId,
            duration = 1,
            schedule = Schedule.SatSun,
            courseName = "Học vẽ cho trẻ nhỏ",
            startDate = DateTime.ParseExact("12/10/2024", "dd/MM/yyyy", null),
            price = 1000,
            courseId = Guid.NewGuid(),
            mentorId = vipMentorDetails.mentorDetailsId,
            beginingClass = new TimeSpan(15,30,00),
            endingClass = new TimeSpan(17,30,00)
        };
        var course8 = new Course()
        {
            subjectId = publicSpeaking.subjectId,
            duration = 1,
            schedule = Schedule.SatSun,
            courseName = "Học vẽ cho học sinh cấp 1",
            startDate = DateTime.ParseExact("12/10/2024", "dd/MM/yyyy", null),
            price = 1000,
            courseId = Guid.NewGuid(),
            mentorId = vipMentorDetails2.mentorDetailsId,
            beginingClass = new TimeSpan(15,30,00),
            endingClass = new TimeSpan(17,30,00)
        };
        
        var course9 = new Course()
        {
            subjectId = publicSpeaking.subjectId,
            duration = 1,
            schedule = Schedule.SatSun,
            courseName = "Học vẽ cho học sinh cấp 2",
            startDate = DateTime.ParseExact("12/10/2024", "dd/MM/yyyy", null),
            price = 1000,
            courseId = Guid.NewGuid(),
            mentorId = vipMentorDetails.mentorDetailsId,
            beginingClass = new TimeSpan(15,30,00),
            endingClass = new TimeSpan(17,30,00)
        };
        
        List<Course> courseList = new List<Course>()
        {
            course1,
            course2,
            course3,
            course4,
            course5,
            course6,
            course7,
            course8,
            course9,
            
        };
        var class1 = new Class()
        {
            mentorId = course1.mentorId,
            classId = course1.courseId,
            courseId = course1.courseId,
            className = course1.courseName
        };
        var class2 = new Class()
        {
            mentorId = course2.mentorId,
            classId = course2.courseId,
            courseId = course2.courseId,
            className = course2.courseName
        };
        var class3 = new Class()
        {
            mentorId = course3.mentorId,
            classId = course3.courseId,
            courseId = course3.courseId,
            className = course3.courseName
        };
        var class4 = new Class()
        {
            mentorId = course4.mentorId,
            classId = course4.courseId,
            courseId = course4.courseId,
            className = course4.courseName
        };
        var class5 = new Class()
        {
            mentorId = course5.mentorId,
            classId = course5.courseId,
            courseId = course5.courseId,
            className = course5.courseName
        };
        var class6= new Class()
        {
            mentorId = course6.mentorId,
            classId = course6.courseId,
            courseId = course6.courseId,
            className = course6.courseName
        };
        var class7 = new Class()
        {
            mentorId = course7.mentorId,
            classId = course7.courseId,
            courseId = course7.courseId,
            className = course7.courseName
        };
        var class8 = new Class()
        {
            mentorId = course8.mentorId,
            classId = course8.courseId,
            courseId = course8.courseId,
            className = course8.courseName
        };
        var class9 = new Class()
        {
            mentorId = course9.mentorId,
            classId = course9.courseId,
            courseId = course9.courseId,
            className = course9.courseName
        };

        List<Class> classes = new List<Class>()
        {
            class1,
            class2,
            class3,
            class4,
            class5,
            class6,
            class7,
            class8,
            class9,
        }; 
        await _context.Users.AddRangeAsync(users);
        await _context.MentorDetails.AddRangeAsync(mentorDetailList);
        await _context.Subjects.AddRangeAsync(monhoc);
        await _context.Courses.AddRangeAsync(courseList);
        await _context.Classes.AddRangeAsync(classes);
        await _context.SaveChangesAsync();
    }
    
}
public static class DatabaseInitialiserExtension
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        // Create IServiceScope to resolve service scope
        using var scope = app.Services.CreateScope();
        var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitialiser>();

        await initializer.InitialiseAsync();

        // Try to seeding data
        await initializer.SeedAsync();
    }
}