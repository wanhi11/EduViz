using System.ComponentModel.DataAnnotations;

namespace EduViz.Common.Payloads.Request;

public class PayOSPaymentRequest
{
    public string OrderCode { get; set; }
    public int Amount { get; set; }
    public string Description { get; set; }
    public List<CreatePaymentItem> Items { get; set; }
    public string CancelUrl { get; set; }
    public string ReturnUrl { get; set; }
}
public class CreatePaymentItem
{
    public string Name { get; set; }
    public int Quantity { get; set; }
    public int Price { get; set; }
}
