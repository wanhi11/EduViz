using EduViz.Entities;
using EduViz.Enums;
using EduViz.Helpers;
using Microsoft.EntityFrameworkCore;

namespace EduViz.Data;

public class DataInit : IDataInitialiser
{
    private readonly ApplicationDbContext _context;

    public DataInit(ApplicationDbContext context)
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
            email = "kieutrongkhanhmentor@gmail.com",
            userName = "Kiều Trọng Khánh Mentor",
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
        var hoihoa = new Subject()
        {
            subjectId = Guid.NewGuid(),
            subjectName = "Vẽ"
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
            subjectName = "Lập trình"
        };
        List<Subject> subjects = new List<Subject>()
        {
            math,ly,hoa,van,su,dia,code,hoihoa
        };
        var course1 = new Course()
        {
            meetUrl = "meet.google.com/wev-rpts-kzo",
            subjectId = math.subjectId,
            duration = 1,
            schedule = Schedule.MonWedFri,
            courseName = "Toán 10 nâng cao thầy Hương",
            startDate = DateTime.ParseExact("12/11/2024", "dd/MM/yyyy", null),
            price = 500000,
            courseId = Guid.NewGuid(),
            mentorId = vipMen1.mentorDetailsId,
            beginingClass = new TimeSpan(15,30,00),
            endingClass = new TimeSpan(17,30,00),
            picture = "https://th.bing.com/th/id/OIP.iMc-4zWQxoLwh0j7k_LBFwHaD3?w=360&h=180&c=7&r=0&o=5&dpr=1.3&pid=1.7"
        };
        var course2 = new Course()
        {
            meetUrl = "meet.google.com/oyq-xchs-spg",
            subjectId = ly.subjectId,
            duration = 1,
            schedule = Schedule.MonWedFri,
            courseName = "Lý 10 nâng cao thầy Hương",
            startDate = DateTime.ParseExact("12/11/2024", "dd/MM/yyyy", null),
            price = 500000,
            courseId = Guid.NewGuid(),
            mentorId = vipMen1.mentorDetailsId,
            beginingClass = new TimeSpan(13,15,00),
            endingClass = new TimeSpan(15,15,00),
            picture = "https://th.bing.com/th/id/OIP.TWeF8-TvjTagzBkwtWrLTwHaE8?w=290&h=193&c=7&r=0&o=5&dpr=1.3&pid=1.7"
        };
        var course3 = new Course()
        {
            meetUrl = "meet.google.com/pfc-ohea-vnz",
            subjectId = hoa.subjectId,
            duration = 1,
            schedule = Schedule.MonWedFri,
            courseName = "Hóa 10 nâng cao thầy Hương",
            startDate = DateTime.ParseExact("12/11/2024", "dd/MM/yyyy", null),
            price = 500000,
            courseId = Guid.NewGuid(),
            mentorId = vipMen1.mentorDetailsId,
            beginingClass = new TimeSpan(17,45,00),
            endingClass = new TimeSpan(17,45,00),
            picture = "data:image/jpeg;base64,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"
        };
        var course4 = new Course()
        {
            meetUrl = "meet.google.com/ckg-znkb-bvz",
            subjectId = math.subjectId,
            duration = 1,
            schedule = Schedule.TueThuSat,
            courseName = "Toán 10 cô Trân",
            startDate = DateTime.ParseExact("12/11/2024", "dd/MM/yyyy", null),
            price = 550000,
            courseId = Guid.NewGuid(),
            mentorId = vipMen2.mentorDetailsId,
            beginingClass = new TimeSpan(15,30,00),
            endingClass = new TimeSpan(17,30,00),
            picture = "https://th.bing.com/th/id/OIP.I0C7c8l16LPGhKkdE_py3wHaE7?pid=ImgDet&w=184&h=122&c=7&dpr=1.3"
        };
        var course5 = new Course()
        {
            meetUrl = "meet.google.com/nqk-iwbq-ajy",
            subjectId = ly.subjectId,
            duration = 1,
            schedule = Schedule.SatSun,
            courseName = "Lý 10 cô Trân",
            startDate = DateTime.ParseExact("12/11/2024", "dd/MM/yyyy", null),
            price = 550000,
            courseId = Guid.NewGuid(),
            mentorId = vipMen2.mentorDetailsId,
            beginingClass = new TimeSpan(13,15,00),
            endingClass = new TimeSpan(15,15,00),
            picture = "data:image/jpeg;base64,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"
        };
        var course6 = new Course()
        {
            meetUrl = "meet.google.com/qbo-weui-ztf",
            subjectId = math.subjectId,
            duration = 1,
            schedule = Schedule.MonWedFri,
            courseName = "Hóa 10 cô Trân",
            startDate = DateTime.ParseExact("12/11/2024", "dd/MM/yyyy", null),
            price = 450000,
            courseId = Guid.NewGuid(),
            mentorId = vipMen2.mentorDetailsId,
            beginingClass = new TimeSpan(17,45,00),
            endingClass = new TimeSpan(17,45,00),
            picture = "data:image/jpeg;base64,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"
        };
        var course7 = new Course()
        {
            meetUrl = "meet.google.com/wev-rpts-kzo",
            subjectId = math.subjectId,
            duration = 1,
            schedule = Schedule.MonWedFri,
            courseName = "Toán 10 nâng cao thầy Hương",
            startDate = DateTime.ParseExact("12/11/2024", "dd/MM/yyyy", null),
            price = 500000,
            courseId = Guid.NewGuid(),
            mentorId = vipMen1.mentorDetailsId,
            beginingClass = new TimeSpan(15,30,00),
            endingClass = new TimeSpan(17,30,00),
            picture = "https://th.bing.com/th/id/OIP.iMc-4zWQxoLwh0j7k_LBFwHaD3?w=360&h=180&c=7&r=0&o=5&dpr=1.3&pid=1.7"
        };
        var course8 = new Course()
        {
            meetUrl = "meet.google.com/oyq-xchs-spg",
            subjectId = ly.subjectId,
            duration = 1,
            schedule = Schedule.MonWedFri,
            courseName = "Lý 10 nâng cao thầy Hương",
            startDate = DateTime.ParseExact("12/11/2024", "dd/MM/yyyy", null),
            price = 500000,
            courseId = Guid.NewGuid(),
            mentorId = vipMen1.mentorDetailsId,
            beginingClass = new TimeSpan(13,15,00),
            endingClass = new TimeSpan(15,15,00),
            picture = "https://th.bing.com/th/id/OIP.TWeF8-TvjTagzBkwtWrLTwHaE8?w=290&h=193&c=7&r=0&o=5&dpr=1.3&pid=1.7"
        };
        var course9 = new Course()
        {
            meetUrl = "meet.google.com/pfc-ohea-vnz",
            subjectId = math.subjectId,
            duration = 1,
            schedule = Schedule.MonWedFri,
            courseName = "Hóa 10 nâng cao thầy Hương",
            startDate = DateTime.ParseExact("12/11/2024", "dd/MM/yyyy", null),
            price = 500000,
            courseId = Guid.NewGuid(),
            mentorId = vipMen1.mentorDetailsId,
            beginingClass = new TimeSpan(17,45,00),
            endingClass = new TimeSpan(17,45,00),
            picture = "data:image/jpeg;base64,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"
        };
        var course10 = new Course()
        {
            meetUrl = "meet.google.com/wev-rpts-kzo",
            subjectId = math.subjectId,
            duration = 1,
            schedule = Schedule.MonWedFri,
            courseName = "Toán 10 nâng cao thầy Hương",
            startDate = DateTime.ParseExact("12/11/2024", "dd/MM/yyyy", null),
            price = 500000,
            courseId = Guid.NewGuid(),
            mentorId = vipMen1.mentorDetailsId,
            beginingClass = new TimeSpan(15,30,00),
            endingClass = new TimeSpan(17,30,00),
            picture = "https://th.bing.com/th/id/OIP.iMc-4zWQxoLwh0j7k_LBFwHaD3?w=360&h=180&c=7&r=0&o=5&dpr=1.3&pid=1.7"
        };
        var course11 = new Course()
        {
            meetUrl = "meet.google.com/wev-rpts-kzo",
            subjectId = math.subjectId,
            duration = 1,
            schedule = Schedule.MonWedFri,
            courseName = "Toán 10 nâng cao thầy Hương",
            startDate = DateTime.ParseExact("12/11/2024", "dd/MM/yyyy", null),
            price = 500000,
            courseId = Guid.NewGuid(),
            mentorId = vipMen1.mentorDetailsId,
            beginingClass = new TimeSpan(15,30,00),
            endingClass = new TimeSpan(17,30,00),
            picture = "https://th.bing.com/th/id/OIP.iMc-4zWQxoLwh0j7k_LBFwHaD3?w=360&h=180&c=7&r=0&o=5&dpr=1.3&pid=1.7"
        };
        var course12 = new Course()
        {
            meetUrl = "meet.google.com/wev-rpts-kzo",
            subjectId = math.subjectId,
            duration = 1,
            schedule = Schedule.MonWedFri,
            courseName = "Toán 10 nâng cao thầy Hương",
            startDate = DateTime.ParseExact("12/11/2024", "dd/MM/yyyy", null),
            price = 500000,
            courseId = Guid.NewGuid(),
            mentorId = vipMen1.mentorDetailsId,
            beginingClass = new TimeSpan(15,30,00),
            endingClass = new TimeSpan(17,30,00),
            picture = "https://th.bing.com/th/id/OIP.iMc-4zWQxoLwh0j7k_LBFwHaD3?w=360&h=180&c=7&r=0&o=5&dpr=1.3&pid=1.7"
        };
        var course13 = new Course()
        {
            meetUrl = "meet.google.com/wev-rpts-kzo",
            subjectId = math.subjectId,
            duration = 1,
            schedule = Schedule.MonWedFri,
            courseName = "Toán 10 nâng cao thầy Hương",
            startDate = DateTime.ParseExact("12/11/2024", "dd/MM/yyyy", null),
            price = 500000,
            courseId = Guid.NewGuid(),
            mentorId = vipMen1.mentorDetailsId,
            beginingClass = new TimeSpan(15,30,00),
            endingClass = new TimeSpan(17,30,00),
            picture = "https://th.bing.com/th/id/OIP.iMc-4zWQxoLwh0j7k_LBFwHaD3?w=360&h=180&c=7&r=0&o=5&dpr=1.3&pid=1.7"
        };
        var course14 = new Course()
        {
            meetUrl = "meet.google.com/wev-rpts-kzo",
            subjectId = math.subjectId,
            duration = 1,
            schedule = Schedule.MonWedFri,
            courseName = "Toán 10 nâng cao thầy Hương",
            startDate = DateTime.ParseExact("12/11/2024", "dd/MM/yyyy", null),
            price = 500000,
            courseId = Guid.NewGuid(),
            mentorId = vipMen1.mentorDetailsId,
            beginingClass = new TimeSpan(15,30,00),
            endingClass = new TimeSpan(17,30,00),
            picture = "https://th.bing.com/th/id/OIP.iMc-4zWQxoLwh0j7k_LBFwHaD3?w=360&h=180&c=7&r=0&o=5&dpr=1.3&pid=1.7"
        };
        var course15 = new Course()
        {
            meetUrl = "meet.google.com/wev-rpts-kzo",
            subjectId = math.subjectId,
            duration = 1,
            schedule = Schedule.MonWedFri,
            courseName = "Toán 10 nâng cao thầy Hương",
            startDate = DateTime.ParseExact("12/11/2024", "dd/MM/yyyy", null),
            price = 500000,
            courseId = Guid.NewGuid(),
            mentorId = vipMen1.mentorDetailsId,
            beginingClass = new TimeSpan(15,30,00),
            endingClass = new TimeSpan(17,30,00),
            picture = "https://th.bing.com/th/id/OIP.iMc-4zWQxoLwh0j7k_LBFwHaD3?w=360&h=180&c=7&r=0&o=5&dpr=1.3&pid=1.7"
        };
        var course16 = new Course()
        {
            meetUrl = "meet.google.com/wev-rpts-kzo",
            subjectId = math.subjectId,
            duration = 1,
            schedule = Schedule.MonWedFri,
            courseName = "Toán 10 nâng cao thầy Hương",
            startDate = DateTime.ParseExact("12/11/2024", "dd/MM/yyyy", null),
            price = 500000,
            courseId = Guid.NewGuid(),
            mentorId = vipMen1.mentorDetailsId,
            beginingClass = new TimeSpan(15,30,00),
            endingClass = new TimeSpan(17,30,00),
            picture = "https://th.bing.com/th/id/OIP.iMc-4zWQxoLwh0j7k_LBFwHaD3?w=360&h=180&c=7&r=0&o=5&dpr=1.3&pid=1.7"
        };
        var course17 = new Course()
        {
            meetUrl = "meet.google.com/wev-rpts-kzo",
            subjectId = math.subjectId,
            duration = 1,
            schedule = Schedule.MonWedFri,
            courseName = "Toán 10 nâng cao thầy Hương",
            startDate = DateTime.ParseExact("12/11/2024", "dd/MM/yyyy", null),
            price = 500000,
            courseId = Guid.NewGuid(),
            mentorId = vipMen1.mentorDetailsId,
            beginingClass = new TimeSpan(15,30,00),
            endingClass = new TimeSpan(17,30,00),
            picture = "https://th.bing.com/th/id/OIP.iMc-4zWQxoLwh0j7k_LBFwHaD3?w=360&h=180&c=7&r=0&o=5&dpr=1.3&pid=1.7"
        };
        var course18 = new Course()
        {
            meetUrl = "meet.google.com/wev-rpts-kzo",
            subjectId = math.subjectId,
            duration = 1,
            schedule = Schedule.MonWedFri,
            courseName = "Toán 10 nâng cao thầy Hương",
            startDate = DateTime.ParseExact("12/11/2024", "dd/MM/yyyy", null),
            price = 500000,
            courseId = Guid.NewGuid(),
            mentorId = vipMen1.mentorDetailsId,
            beginingClass = new TimeSpan(15,30,00),
            endingClass = new TimeSpan(17,30,00),
            picture = "https://th.bing.com/th/id/OIP.iMc-4zWQxoLwh0j7k_LBFwHaD3?w=360&h=180&c=7&r=0&o=5&dpr=1.3&pid=1.7"
        };
        var course19 = new Course()
        {
            meetUrl = "meet.google.com/wev-rpts-kzo",
            subjectId = math.subjectId,
            duration = 1,
            schedule = Schedule.MonWedFri,
            courseName = "Toán 10 nâng cao thầy Hương",
            startDate = DateTime.ParseExact("12/11/2024", "dd/MM/yyyy", null),
            price = 500000,
            courseId = Guid.NewGuid(),
            mentorId = vipMen1.mentorDetailsId,
            beginingClass = new TimeSpan(15,30,00),
            endingClass = new TimeSpan(17,30,00),
            picture = "https://th.bing.com/th/id/OIP.iMc-4zWQxoLwh0j7k_LBFwHaD3?w=360&h=180&c=7&r=0&o=5&dpr=1.3&pid=1.7"
        };
        var course20= new Course()
        {
            meetUrl = "meet.google.com/wev-rpts-kzo",
            subjectId = math.subjectId,
            duration = 1,
            schedule = Schedule.MonWedFri,
            courseName = "Toán 10 nâng cao thầy Hương",
            startDate = DateTime.ParseExact("12/11/2024", "dd/MM/yyyy", null),
            price = 500000,
            courseId = Guid.NewGuid(),
            mentorId = vipMen1.mentorDetailsId,
            beginingClass = new TimeSpan(15,30,00),
            endingClass = new TimeSpan(17,30,00),
            picture = "https://th.bing.com/th/id/OIP.iMc-4zWQxoLwh0j7k_LBFwHaD3?w=360&h=180&c=7&r=0&o=5&dpr=1.3&pid=1.7"
        };
        
    }
}