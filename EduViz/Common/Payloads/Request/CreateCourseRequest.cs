using EduViz.Dtos;
using EduViz.Enums;
using EduViz.Exceptions;

namespace EduViz.Common.Payloads.Request;

public class CreateCourseRequest
{
    public string courseName { get; set; }
    public string SubjectName { get; set; }
    public decimal Price { get; set; }
    
    public string WeekSchedule { get; set; }
    
    public string StartDate { get; set; }
    public int Duration { get; set; }
    public IFormFile? Picture { get; set; }
}

public static class CreateCourseRequestExtensions
{
    public static CourseModel ToCourseModel(this CreateCourseRequest courseRequest)
    {
        var currentDate = DateTime.ParseExact(courseRequest.StartDate, "dd/MM/yyyy", null);
        if (currentDate < DateTime.Now)
        {
            throw new BadRequestException("StartDate is passed");
        }

        var courseModel = new CourseModel()
        {
            CourseId = Guid.NewGuid(),
            CourseName = courseRequest.courseName,
            Price = courseRequest.Price,
            StartDate = currentDate,
            Schedule = (Schedule) Enum.Parse(typeof(Schedule), courseRequest.WeekSchedule),
            Duration = courseRequest.Duration,
        };
        return courseModel;
    }
}