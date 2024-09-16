using EduViz.Common.Payloads;
using EduViz.Common.Payloads.Request;
using EduViz.Common.Payloads.Response;
using EduViz.Dtos;
using EduViz.Entities;
using EduViz.Exceptions;
using EduViz.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Net.payOS;
using Net.payOS.Types;
using Newtonsoft.Json;

namespace EduViz.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly PaymentService _paymentService;
    private readonly MentorDetailService _mentorDetailService;
    private readonly UserService _userService;
    private readonly PayOsPaymentService _payOSService;
    private readonly UpgradeOrderDetailService _upgradeService;

    public PaymentController(PaymentService paymentService, MentorDetailService mentorDetailService,
        UserService userService, PayOsPaymentService payOSService,UpgradeOrderDetailService upgradeService)
    {
        _paymentService = paymentService;
        _mentorDetailService = mentorDetailService;
        _userService = userService;
        _payOSService = payOSService;
        _upgradeService = upgradeService;
    }

    [HttpGet("premium-package")]
    public IActionResult GetPremiumPackage()
    {
        return Ok(ApiResult<GetPremiumPackageResponse>.Succeed(new GetPremiumPackageResponse()
        {
            PremiumPackageInfos = new List<PremiumPackageInfo>()
            {
                new PremiumPackageInfo()
                {
                    Amout = 10000,
                    MonthDuraion = 1,
                    PackageName = "Goi 1 thang"
                },
                new PremiumPackageInfo()
                {
                    Amout = 25000,
                    MonthDuraion = 3,
                    PackageName = "Goi 3 thang"
                },

                new PremiumPackageInfo()
                {
                    Amout = 50000,
                    MonthDuraion = 6,
                    PackageName = "Goi 6 thang"
                }
            }
        }));
    }

    [HttpPost("create-payment-link")]
    public async Task<IActionResult> CreatePaymentLink([FromBody] CreatePaymentResult paymentRequest)
    {
        return Ok();
    }

    [HttpPost("upgrade-to-vip")]
    [Authorize(Roles ="Mentor")]
    public async Task<IActionResult> UpgradeToVip([FromBody] VipUpgradeRequest request)
    {
        var result = await _payOSService.CreateVipUpgradePaymentLinkAsync(request);
        if (result is null)
        {
            throw new BadRequestException("Something went wrong");
        }

        return Ok(ApiResult<PayOSPaymentData>.Succeed(new PayOSPaymentData()
        {
            PaymentLink = result.PayOSResult.checkoutUrl,
            Signature = result.Signature
        }));
    }

    [HttpPost("payos-webhook")]
    public async Task<IActionResult> HandlePayOSWebhook([FromBody] WebhookType webhookBody)
    {
        long code = webhookBody.data.orderCode;
        var upgradeOrder = _upgradeService.FindOrderByCode(code);
        await _upgradeService.UpdateStatus(code);
        string result = await _payOSService.ProcessPaymentWebhookAsync(webhookBody,upgradeOrder.MentorDetailsID);
        return Ok(ApiResult<PayosWebHookReponse>.Succeed(new PayosWebHookReponse()
        {
            Message = result
        }));
    }
    [HttpPost("confirm-webhook")]
    public IActionResult ConfirmWebhookUrl([FromBody] string webhookUrl)
    {
        var result = _payOSService.ConfirmWebhook(webhookUrl);

        if (result.Equals("success"))
        {
            return Ok(new { message = "Webhook URL confirmed successfully." });
        }

        return BadRequest(new { message = "Failed to confirm Webhook URL." });
    }
    [HttpPost("receive-hook")]
    public IActionResult ReceiveHook([FromBody] object requestBody)
    {
        // Log the request body (you can change 'object' to the actual expected type if known)
        Console.WriteLine(JsonConvert.SerializeObject(requestBody));

        // Return a JSON response (you can return an actual object if needed)
        return Ok(new { message = "Webhook received successfully" });
    }
}   