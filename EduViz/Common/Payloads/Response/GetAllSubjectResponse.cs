using EduViz.Dtos;

namespace EduViz.Common.Payloads.Response;

public class GetAllSubjectResponse
{
    public List<SubjectModel> listSubject { get; set; }
}