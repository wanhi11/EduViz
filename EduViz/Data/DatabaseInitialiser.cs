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

        var admin = new User()
        {
            userId = Guid.NewGuid(),
            email = "admin@gmail.com",
            userName = "Admin",
            password = SecurityUtil.Hash("123456"),
            role = Role.Admin,
        };
        
        var stu1 = new User()
        {
            userId = Guid.NewGuid(),
            email = "nguyenvuquanghuy@gmail.com",
            userName = "Nguyen Vu Quang Huy",
            password = SecurityUtil.Hash("123456"),
            role = Role.Student
        };
        var stu2 = new User()
        {
            userId = Guid.NewGuid(),
            email = "nguyenquangthoai@gmail.com",
            userName = "Nguyen Quang Thoai",
            password = SecurityUtil.Hash("123456"),
            role = Role.Student
        };
        var stu3 = new User()
        {
            userId = Guid.NewGuid(),
            email = "nguyenquangteo@gmail.com",
            userName = "Nguyen Quang Teo",
            password = SecurityUtil.Hash("123456"),
            role = Role.Student
        };
        var stu4 = new User()
        {
            userId = Guid.NewGuid(),
            email = "buithetam@gmail.com",
            userName = "Bùi Thế Tâm",
            password = SecurityUtil.Hash("123456"),
            role = Role.Student
        };
        var stu5 = new User()
        {
            userId = Guid.NewGuid(),
            email = "doanvietthanh@gmail.com",
            userName = "Đoàn Viết Thanh",
            password = SecurityUtil.Hash("123456"),
            role = Role.Student
        };
        var stu6 = new User()
        {
            userId = Guid.NewGuid(),
            email = "lexuanphuoc@gmail.com",
            userName = "Lê Xuân Phước",
            password = SecurityUtil.Hash("123456"),
            role = Role.Student
        };
        var stu7 = new User()
        {
            userId = Guid.NewGuid(),
            email = "trantruongvan@gmail.com",
            userName = "Trần Trương Văn",
            password = SecurityUtil.Hash("123456"),
            role = Role.Student
        };
        var stu8 = new User()
        {
            userId = Guid.NewGuid(),
            email = "lequangdung@gmail.com",
            userName = "Lê Quang Dũng",
            password = SecurityUtil.Hash("123456"),
            role = Role.Student
        };
        var stu9 = new User()
        {
            userId = Guid.NewGuid(),
            email = "nguyenngothanhnha@gmail.com",
            userName = "Nguyễn Ngô Thanh Nhã",
            password = SecurityUtil.Hash("123456"),
            role = Role.Student
        };
        var stu10 = new User()
        {
            userId = Guid.NewGuid(),
            email = "trantrunghieu@gmail.com",
            userName = "Trần Trung Hiếu",
            password = SecurityUtil.Hash("123456"),
            role = Role.Student
        };
        var stu11 = new User()
        {
            userId = Guid.NewGuid(),
            email = "nguyenminhtam@gmail.com",
            userName = "Nguyễn Minh Tâm",
            password = SecurityUtil.Hash("123456"),
            role = Role.Student
        };
        var men1 = new User()
        {
            userId = Guid.NewGuid(),
            email = "trandinhhuong@gmail.com",
            userName = "Trần Đình Hương",
            password = SecurityUtil.Hash("123456"),
            role = Role.Mentor
        };
        var men2 = new User()
        {
            userId = Guid.NewGuid(),
            email = "huynhtotran@gmail.com",
            userName = "Huỳnh Tố Trân",
            password = SecurityUtil.Hash("123456"),
            role = Role.Mentor
        };

        var men3 = new User()
        {
            userId = Guid.NewGuid(),
            email = "dinhconghuyhoang@gmail.com",
            userName = "Đinh Công Huy Hoàng",
            password = SecurityUtil.Hash("123456"),
            role = Role.Mentor
        };
        var men4 = new User()
        {
            userId = Guid.NewGuid(),
            email = "nguyenthitruchoa@gmail.com",
            userName = "Nguyễn Thị Trúc Hoa",
            password = SecurityUtil.Hash("123456"),
            role = Role.Mentor
        };
        var men5 = new User()
        {
            userId = Guid.NewGuid(),
            email = "tranthithucuc@gmail.com",
            userName = "Trần Thị Thu Cúc",
            password = SecurityUtil.Hash("123456"),
            role = Role.Mentor
        };
        var men6 = new User()
        {
            userId = Guid.NewGuid(),
            email = "mrHLong@gmail.com",
            userName = "Trần Hoàng Long ",
            password = SecurityUtil.Hash("123456"),
            role = Role.Mentor
        };
        var men7 = new User()
        {
            userId = Guid.NewGuid(),
            email = "adamsmith@gmail.com",
            userName = "Adam Smith",
            password = SecurityUtil.Hash("123456"),
            role = Role.Mentor
        };
        var men8 = new User()
        {
            userId = Guid.NewGuid(),
            email = "artforlife@gmail.com",
            userName = "Trần Ngọc Thoa",
            password = SecurityUtil.Hash("123456"),
            role = Role.Mentor
        };
        var men9 = new User()
        {
            userId = Guid.NewGuid(),
            email = "toidicodedao@gmail.com",
            userName = "Phạm Huy Hoàng",
            password = SecurityUtil.Hash("123456"),
            role = Role.Mentor
        };
        List<User> users = new List<User>()
        {
            stu1,stu2,stu3,stu4,stu5,stu6,stu7,stu8,
            stu9,stu10,stu11,
            men1,men2,men3,men4,men5,men6,men7,men8,men9
        };
        var norMen1 = new MentorDetails()
        {
            userId = men3.userId,
            mentorDetailsId = Guid.NewGuid(),
            vipExpirationDate =  DateTime.ParseExact("12/09/2024", "dd/MM/yyyy", null)
        };
        var norMen2 = new MentorDetails()
        {
            userId = men4.userId,
            mentorDetailsId = Guid.NewGuid(),
            vipExpirationDate =  DateTime.ParseExact("12/09/2024", "dd/MM/yyyy", null)
        };
        var norMen3 = new MentorDetails()
        {
            userId = men5.userId,
            mentorDetailsId = Guid.NewGuid(),
            vipExpirationDate =  DateTime.ParseExact("12/09/2024", "dd/MM/yyyy", null)
        };
        var norMen4 = new MentorDetails()
        {
            userId = men7.userId,
            mentorDetailsId = Guid.NewGuid(),
            vipExpirationDate =  DateTime.ParseExact("12/09/2024", "dd/MM/yyyy", null)
        };
        var norMen5 = new MentorDetails()
        {
            userId = men9.userId,
            mentorDetailsId = Guid.NewGuid(),
            vipExpirationDate =  DateTime.ParseExact("12/09/2024", "dd/MM/yyyy", null)
        };

        var vipMen1 = new MentorDetails()
        {
            userId = men1.userId,
            mentorDetailsId = Guid.NewGuid(),
            vipExpirationDate = DateTime.ParseExact("12/01/2025", "dd/MM/yyyy", null)
        };
        var vipMen2 = new MentorDetails()
        {
            userId = men2.userId,
            mentorDetailsId = Guid.NewGuid(),
            vipExpirationDate = DateTime.ParseExact("12/01/2025", "dd/MM/yyyy", null)
        };
        var vipMen3 = new MentorDetails()
        {
            userId = men6.userId,
            mentorDetailsId = Guid.NewGuid(),
            vipExpirationDate = DateTime.ParseExact("12/01/2025", "dd/MM/yyyy", null)
        };
        var vipMen4 = new MentorDetails()
        {
            userId = men8.userId,
            mentorDetailsId = Guid.NewGuid(),
            vipExpirationDate = DateTime.ParseExact("12/01/2025", "dd/MM/yyyy", null)
        };
        List<MentorDetails> mentorDetailsList = new List<MentorDetails>()
        {
            norMen1,norMen2,norMen3,norMen4,norMen5,
            vipMen1,vipMen2,vipMen3,vipMen4
        };
        var math = new Subject()
        {
            subjectId = Guid.NewGuid(),
            subjectName = "Toán"
        };
        var hoa = new Subject()
        {
            subjectId = Guid.NewGuid(),
            subjectName = "Hóa"
        };
        var english = new Subject()
        {
            subjectId = Guid.NewGuid(),
            subjectName = "Tiếng Anh"
        };
        
        var ly = new Subject()
        {
            subjectId = Guid.NewGuid(),
            subjectName = "Lý"
        };
        
        var van = new Subject()
        {
            subjectId = Guid.NewGuid(),
            subjectName = "Ngữ Văn"
        };
        var sinh = new Subject()
        {
            subjectId = Guid.NewGuid(),
            subjectName = "Sinh"
        };
        var su = new Subject()
        {
            subjectId = Guid.NewGuid(),
            subjectName = "Sử"
        };
        var dia = new Subject()
        {
            subjectId = Guid.NewGuid(),
            subjectName = "Địa"
        };
        var code = new Subject()
        {
            subjectId = Guid.NewGuid(),
            subjectName = "Tin Học"
        };
        List<Subject> subjects = new List<Subject>()
        {
            math,ly,hoa,van,su,dia,code,sinh
        };
        var course1 = new Course()
        {
            meetUrl = "meet.google.com/wev-rpts-kzo",
            subjectId = math.subjectId,
            duration = 1,
            schedule = Schedule.MonWedFri,
            courseName = "Toán 10 nâng cao thầy Hương",
            startDate = DateTime.ParseExact("12/11/2024", "dd/MM/yyyy", null),
            price = 10000,
            courseId = Guid.NewGuid(),
            mentorId = vipMen1.mentorDetailsId,
            beginingClass = new TimeSpan(15,30,00),
            endingClass = new TimeSpan(17,30,00),
            picture = "https://th.bing.com/th/id/OIP.iMc-4zWQxoLwh0j7k_LBFwHaD3?w=360&h=180&c=7&r=0&o=5&dpr=1.3&pid=1.7"
        };
        var course2 = new Course()
        {
            meetUrl = "meet.google.com/wev-rpts-kzo",
            subjectId = math.subjectId,
            duration = 1,
            schedule = Schedule.MonWedFri,
            courseName = "Toán 10 căn bản",
            startDate = DateTime.ParseExact("12/11/2024", "dd/MM/yyyy", null),
            price = 10000,
            courseId = Guid.NewGuid(),
            mentorId = norMen1.mentorDetailsId,
            beginingClass = new TimeSpan(15,30,00),
            endingClass = new TimeSpan(17,30,00),
            picture = "https://res.cloudinary.com/dbnnedoks/image/upload/v1730042880/b92ne76uq0ri7i5jtd2f.png"
        };
        List<Course> courses = new List<Course>()
        {
            course1,course2
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
        List<Class> classes = new List<Class>()
        {
            class1,class2
        }; 
        await _context.Users.AddRangeAsync(users);
        await _context.MentorDetails.AddRangeAsync(mentorDetailsList);
        await _context.Subjects.AddRangeAsync(subjects);
        await _context.Courses.AddRangeAsync(courses);
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