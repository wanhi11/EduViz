using System.Security.Cryptography.Xml;
using Net.payOS.Types;

namespace EduViz.Common.Payloads.Response;

public class PayOSPaymentResponse
{
    public CreatePaymentResult payOSResult;
    public string signature;
}

public class PayOSPaymentData
{
    public string paymentLink { get; set; }
    public string signature { get; set; } 
}
