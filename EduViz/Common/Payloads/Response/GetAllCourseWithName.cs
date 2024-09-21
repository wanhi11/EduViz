using EduViz.Dtos;

namespace EduViz.Common.Payloads.Response;

public class GetAllCourseWithName
{
    public List<CourseResponse> ListCourse { get; set; }
}