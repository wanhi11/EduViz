using EduViz.Dtos;
using EduViz.Entities;

namespace EduViz.Common.Payloads.Response;

public class GetClassDetailsReponse
{
    public List<StudentInfoInClass> studentInfoList { get; set; }
    // meetUrl = course.meetUrl,
    // weekSchedule = ConvertEnumHelper.ConvertEnumToDayList(course.Schedule.ToString()),
    // subjectName = subject.SubjectName,
    // duration = course.Duration,
    // beginingClass = course.beginingClass.ToString(@"hh\:mm\:ss"),
    // endingClass = course.endingClass.ToString(@"hh\:mm\:ss")
    public string meetUrl { get; set; }
    public List<string>weekSchedule { get; set; }
    public string subjectName { get; set; }
    public int monthDuration { get; set; }
    public string beginingClass { get; set; }
    public string endingClass { get; set; }
}

public class StudentInfoInClass
{
    public string name { get; set; }
    public string gender { get; set; }
    public double? score { get; set; }
    public int numOfTry { get; set; }
}