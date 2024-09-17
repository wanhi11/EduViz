using EduViz.Dtos;
using EduViz.Entities;

namespace EduViz.Common.Payloads.Response;

public class GetAllCourseResponse
{
    public List<CourseModel> ListCourse { get; set; }
}