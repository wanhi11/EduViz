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
        var currentDate = DateTime.ParseExact(courseRequest.startDate, "dd/MM/yyyy", null);
        if (currentDate < DateTime.Now)
        {
            throw new BadRequestException("StartDate is passed");
        }

        TimeSpan startClass = TimeSpan.Parse(courseRequest.beginingClass);
        TimeSpan endClass = TimeSpan.Parse(courseRequest.endingClass);
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
            StartDate = currentDate,
            Schedule = (Schedule) Enum.Parse(typeof(Schedule), ConvertEnumHelper.ConvertDayListToEnum(courseRequest.weekSchedule)),
            Duration = courseRequest.duration,
            beginingClass = TimeSpan.Parse(courseRequest.beginingClass),
            endingClass = TimeSpan.Parse(courseRequest.endingClass)
        };
        return courseModel;
    }
}