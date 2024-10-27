using EduViz.Enums;

namespace EduViz.Common.Payloads.Response;

public class GetAllPaymentByMentorId
{
    public List<AnalysPayment> payments { get; set; }
}

public class AnalysPayment
{
    public Guid paymentId { get; set; }

    public string studentName { get; set; }

    public string courseName { get; set; }
    public int Amount { get; set; }

    public DateTime PaymentDate { get; set; }
}