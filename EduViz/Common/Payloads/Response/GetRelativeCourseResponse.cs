using EduViz.Dtos;

namespace EduViz.Common.Payloads.Response;

public class GetRelativeCourseResponse
{
    public List<CourseResponse> listRelativeCourse { get; set; }
}