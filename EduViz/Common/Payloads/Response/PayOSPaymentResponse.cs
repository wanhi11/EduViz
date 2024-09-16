using System.Security.Cryptography.Xml;
using Net.payOS.Types;

namespace EduViz.Common.Payloads.Response;

public class PayOSPaymentResponse
{
    public CreatePaymentResult PayOSResult;
    public string Signature;
}

public class PayOSPaymentData
{
    public string PaymentLink { get; set; }
    public string Signature { get; set; } 
}
