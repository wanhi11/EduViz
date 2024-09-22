using System.ComponentModel.DataAnnotations;
using EduViz.Dtos;
using EduViz.Enums;
using EduViz.Exceptions;
using EduViz.Helpers;

namespace EduViz.Common.Payloads.Request;

public class CreateCourseRequest
{
    public string courseName { get; set; }
    public string subjectName { get; set; }
    public decimal price { get; set; }
    
    public List<string> weekSchedule { get; set; }
    
    public string startDate { get; set; }
    public int duration { get; set; }
    public IFormFile? picture { get; set; }
    public string beginingClass { get; set; }
    public string endingClass { get; set; }
}

public static class CreateCourseRequestExtensions
{
    public static CourseModel ToCourseModel(this CreateCourseRequest courseRequest)
    {
        if (courseRequest.price <= 0)
        {
            throw new BadRequestException("Course price must be larger than 0");
        }

        DateTime startDate;
        bool success = DateTime.TryParseExact(
            courseRequest.startDate,
            "dd/MM/yyyy",
            null,
            System.Globalization.DateTimeStyles.None,
            out startDate);
        if (!success)
        {
            throw new BadRequestException("Wrong format date dd/MM/yyyy");
        }
        
        if (startDate < DateTime.Now)
        {
            throw new BadRequestException("StartDate is passed");
        }

        TimeSpan startClass ;
        bool parseBeginHour = TimeSpan.TryParseExact(courseRequest.beginingClass,"hh\\:mm\\:ss",null,out startClass);
        TimeSpan endClass;
        bool parseEndHour= TimeSpan.TryParseExact(courseRequest.endingClass,"hh\\:mm\\:ss",null,out endClass);

        if (!parseBeginHour || !parseEndHour)
        {
            throw new BadRequestException("Format time must be hh:mm:ss");
        }

        TimeSpan neededTime = new TimeSpan(1, 30, 00);
        
        if ((endClass - startClass) <neededTime)
        {
            throw new BadRequestException("Class must be 1h30m long at least");
        }

        var courseModel = new CourseModel()
        {
            CourseId = Guid.NewGuid(),
            CourseName = courseRequest.courseName,
            Price = courseRequest.price,
            StartDate = startDate,
            Schedule = (Schedule) Enum.Parse(typeof(Schedule), ConvertEnumHelper.ConvertDayListToEnum(courseRequest.weekSchedule)),
            Duration = courseRequest.duration,
            beginingClass = TimeSpan.Parse(courseRequest.beginingClass),
            endingClass = TimeSpan.Parse(courseRequest.endingClass)
        };
        return courseModel;
    }
}