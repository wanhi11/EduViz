using EduViz.Dtos;
using EduViz.Enums;
using EduViz.Exceptions;

namespace EduViz.Common.Payloads.Request;

public class CreateCourseRequest
{
    public string CourseName { get; set; }
    public string SubjectName { get; set; }
    public decimal Price { get; set; }
    
    public Schedule WeekSchedule { get; set; }
    
    public DateTime StartDate { get; set; }
    public int Duration { get; set; }
    public IFormFile? Picture { get; set; }
}

public static class CreateCourseRequestExtensions
{
    public static CourseModel ToCourseModel(this CreateCourseRequest courseRequest)
    {
        if (courseRequest.StartDate < DateTime.Now)
        {
            throw new BadRequestException("StartDate is passed");
        }

        var courseModel = new CourseModel()
        {
            CourseId = Guid.NewGuid(),
            CourseName = courseRequest.CourseName,
            Price = courseRequest.Price,
            StartDate = courseRequest.StartDate,
            Schedule = courseRequest.WeekSchedule,
            Duration = courseRequest.Duration,
        };
        return courseModel;
    }
}