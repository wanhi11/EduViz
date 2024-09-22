using System.ComponentModel.DataAnnotations;
using EduViz.Dtos;
using EduViz.Enums;
using EduViz.Exceptions;

namespace EduViz.Common.Payloads.Request;

public class CreateCourseRequest
{
    public string courseName { get; set; }
    public string subjectName { get; set; }
    public decimal price { get; set; }
    
    public string weekSchedule { get; set; }
    
    public string startDate { get; set; }
    public int duration { get; set; }
    public IFormFile? picture { get; set; }
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

        var courseModel = new CourseModel()
        {
            CourseId = Guid.NewGuid(),
            CourseName = courseRequest.courseName,
            Price = courseRequest.price,
            StartDate = currentDate,
            Schedule = (Schedule) Enum.Parse(typeof(Schedule), courseRequest.weekSchedule),
            Duration = courseRequest.duration,
        };
        return courseModel;
    }
}