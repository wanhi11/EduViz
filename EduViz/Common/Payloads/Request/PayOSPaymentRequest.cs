using System.ComponentModel.DataAnnotations;

namespace EduViz.Common.Payloads.Request;

public class PayOSPaymentRequest
{
    public string orderCode { get; set; }
    public int amount { get; set; }
    public string description { get; set; }
    public List<CreatePaymentItem> items { get; set; }
    public string cancelUrl { get; set; }
    public string returnUrl { get; set; }
}
public class CreatePaymentItem
{
    public string name { get; set; }
    public int quantity { get; set; }
    public int price { get; set; }
}
