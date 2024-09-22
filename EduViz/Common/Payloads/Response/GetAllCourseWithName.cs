using EduViz.Dtos;

namespace EduViz.Common.Payloads.Response;

public class GetAllCourseWithName
{
    public List<CourseResponse> listCourse { get; set; }
}