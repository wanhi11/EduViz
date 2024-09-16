using EduViz.Dtos;

namespace EduViz.Common.Payloads.Request;

public class CreateCourseRequest
{
    public string CourseName { get; set; }
    public string SubjectName { get; set; }
    public decimal Price { get; set; }
}

public static class CreateCourseRequestExtensions
{
    public static CourseModel ToCourseModel(this CreateCourseRequest courseRequest)
    {
        var courseModel = new CourseModel()
        {
            CourseId = Guid.NewGuid(),
            CourseName = courseRequest.CourseName,
            Price = courseRequest.Price
        };
        return courseModel;
    }
}