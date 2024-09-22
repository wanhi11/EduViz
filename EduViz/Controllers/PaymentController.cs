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
[Route("api/payment")]
public class PaymentController : ControllerBase
{
    private readonly PaymentService _paymentService;
    private readonly MentorDetailService _mentorDetailService;
    private readonly UserService _userService;
    private readonly PayOsPaymentService _payOSService;
    private readonly UpgradeOrderDetailService _upgradeService;
    private readonly CourseService _courseService;

    public PaymentController(PaymentService paymentService, MentorDetailService mentorDetailService,
        UserService userService, PayOsPaymentService payOSService,UpgradeOrderDetailService upgradeService,
        CourseService courseService)
    {
        _paymentService = paymentService;
        _mentorDetailService = mentorDetailService;
        _userService = userService;
        _payOSService = payOSService;
        _upgradeService = upgradeService;
        _courseService = courseService;
    }

    [HttpGet("premium-package")]
    public IActionResult GetPremiumPackage()
    {
        return Ok(ApiResult<GetPremiumPackageResponse>.Succeed(new GetPremiumPackageResponse()
        {
            premiumPackageInfos = new List<PremiumPackageInfo>()
            {
                new PremiumPackageInfo()
                {
                    amout = 10000,
                    monthDuraion = 1,
                    packageName = "Goi 1 thang"
                },
                new PremiumPackageInfo()
                {
                    amout = 25000,
                    monthDuraion = 3,
                    packageName = "Goi 3 thang"
                },

                new PremiumPackageInfo()
                {
                    amout = 50000,
                    monthDuraion = 6,
                    packageName = "Goi 6 thang"
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
            paymentLink = result.payOSResult.checkoutUrl,
            signature = result.signature
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
            message = result
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

    [HttpPost("purchase-course")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> PurchaseCourse([FromBody] CoursePurchaseRequest purchasereq)
    {
        Request.Headers.TryGetValue("Authorization", out var token);
        token = token.ToString().Split()[1];
        var user = _userService.GetUserInToken(token);
        var result = await _payOSService.CreatePurchaseCourseAsync(purchasereq,user.UserId);
        if (result is null)
        {
            throw new BadRequestException("Something went wrong");
        }

        return Ok(ApiResult<PayOSPaymentData>.Succeed(new PayOSPaymentData()
        {
            paymentLink = result.payOSResult.checkoutUrl,
            signature = result.signature
        }));
        
    }


}   