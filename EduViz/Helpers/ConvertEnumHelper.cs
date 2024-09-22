using System.Text.RegularExpressions;

namespace EduViz.Helpers;

public static class ConvertEnumHelper
{
    public static List<string> ConvertEnumToDayList(string enumString)
    {
        // Bước 1: Tạo từ điển ánh xạ từ viết tắt sang tên đầy đủ
        var dayMapping = new Dictionary<string, string>()
        {
            { "Mon", "Monday" },
            { "Tue", "Tuesday" },
            { "Wed", "Wednesday" },
            { "Thu", "Thursday" },
            { "Fri", "Friday" },
            { "Sat", "Saturday" },
            { "Sun", "Sunday" }
        };

        // Bước 2: Sử dụng Regex để chia nhỏ chuỗi theo các từ viết tắt
        var matches = Regex.Matches(enumString, @"Mon|Tue|Wed|Thu|Fri|Sat|Sun");

        // Bước 3: Tạo danh sách các tên đầy đủ của các ngày
        var dayList = new List<string>();
        foreach (Match match in matches)
        {
            if (dayMapping.TryGetValue(match.Value, out var fullDayName))
            {
                dayList.Add(fullDayName);
            }
        }

        return dayList; // Trả về danh sách tên đầy đủ
    }
    public static string ConvertDayListToEnum(List<string> dayList)
    {
        // Bước 1: Tạo từ điển ánh xạ ngược từ tên đầy đủ sang viết tắt
        var reverseDayMapping = new Dictionary<string, string>()
        {
            { "Monday", "Mon" },
            { "Tuesday", "Tue" },
            { "Wednesday", "Wed" },
            { "Thursday", "Thu" },
            { "Friday", "Fri" },
            { "Saturday", "Sat" },
            { "Sunday", "Sun" }
        };

        // Bước 2: Duyệt qua danh sách và chuyển đổi các ngày thành viết tắt
        var enumString = "";
        foreach (var day in dayList)
        {
            if (reverseDayMapping.TryGetValue(day, out var shortDay))
            {
                enumString += shortDay;
            }
        }

        return enumString; // Trả về chuỗi enum
    }
}