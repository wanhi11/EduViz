using EduViz.Dtos;

namespace EduViz.Common.Payloads.Response;

public class GetAllCourseWithName
{
    public List<CourseModel> ListCourse { get; set; }
}