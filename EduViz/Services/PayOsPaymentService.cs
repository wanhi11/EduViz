using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using EduViz.Common.Payloads.Request;
using EduViz.Common.Payloads.Response;
using EduViz.Dtos;
using EduViz.Entities;
using EduViz.Enums;
using EduViz.Exceptions;
using EduViz.Repositories;
using Newtonsoft.Json;
using Net.payOS;
using Net.payOS.Types;

namespace EduViz.Services;

public class PayOsPaymentService
{
    private readonly string _clientId;
    private readonly string _apiKey;
    private readonly string _checksumKey;
    private readonly IConfiguration _configuration;
    private readonly IRepository<MentorDetails, Guid> _mentorRepository;
    private readonly IRepository<UpgradeOrderDetails, Guid> _upgradeRepository;


    public PayOsPaymentService(IConfiguration configuration, IRepository<MentorDetails, Guid> mentorRepository,
        IRepository<User, Guid> userRepository,IRepository<UpgradeOrderDetails,Guid> upgradeRepository)
    {
        _configuration = configuration;
        _clientId = _configuration["PayOS:ClientId"];
        _apiKey = _configuration["PayOS:ApiKey"];
        _checksumKey = _configuration["PayOS:ChecksumKey"];
        _mentorRepository = mentorRepository;
        _upgradeRepository = upgradeRepository;
    }

    public async Task<PayOSPaymentResponse> CreateVipUpgradePaymentLinkAsync(
        VipUpgradeRequest request)
    {

        int amount = GetAmountByMonth(request.MonthDuration);
        List<ItemData> items = new List<ItemData>
        {
            new ItemData("Upgrade Vip "+request.MonthDuration+" Month", 1, GetAmountByMonth(request.MonthDuration))
        };
        long orderCode = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var result = await CreatePaymentLinkAsync(orderCode, amount, "Upgrade Vip "+request.MonthDuration+" Month", items
            , request.CancelUrl, request.ReturnUrl);
        
        var mentorUpgradeDetail = new UpgradeOrderDetails()
        {
            amount = amount,
            orderCode = orderCode,
            paymentStatus = PaymentStatus.Pending,
            mentorDetailsID = request.MentorDetailID,
            packageName = request.MonthDuration+"M"
        };
        await _upgradeRepository.AddAsync(mentorUpgradeDetail);
        if (!(await _upgradeRepository.Commit() > 0))
        {
            throw new BadRequestException("Something went wrong when create upgrade order");
        }
        return result;
    }

    public int GetAmountByMonth(int month)
    {
        if (month == 1)
        {
            return int.Parse(_configuration["PremiumPackagePrice:1M"]);
        }
        else if (month == 3)
        {
            return int.Parse(_configuration["PremiumPackagePrice:3M"]);
        }
        else
        {
            return int.Parse(_configuration["PremiumPackagePrice:6M"]);
        }
    }

    public async Task<PayOSPaymentResponse> CreatePaymentLinkAsync(long orderCode, int amount, string description,
        List<ItemData> items, string cancelUrl, string returnUrl)
    {
        
        PayOS payOS = new PayOS(_clientId, _apiKey, _checksumKey);

        PaymentData paymentData = new PaymentData(orderCode, amount, description, items, cancelUrl, returnUrl);

        var payOSResult = await payOS.createPaymentLink(paymentData);
        return new PayOSPaymentResponse()
        {
            PayOSResult = payOSResult,
            Signature = GenerateSignature(orderCode,amount,description,cancelUrl,returnUrl)
        };
    }

    // Phương thức tính chữ ký HMAC_SHA256
    public string GenerateSignature(long orderCode, int amount, string description, string cancelUrl, string returnUrl)
    {
        string data =
            $"amount={amount}&cancelUrl={cancelUrl}&description={description}&orderCode={orderCode}&returnUrl={returnUrl}";
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_checksumKey));
        return BitConverter.ToString(hmac.ComputeHash(Encoding.UTF8.GetBytes(data))).Replace("-", "").ToLower();
    }
    private int GetMonthsFromDescription(string description)
    {
        var match = Regex.Match(description, @"Upgrade Vip (\d+) Month");

        if (match.Success && int.TryParse(match.Groups[1].Value, out int months))
        {
            return months;
        }
        else
        {
            throw new BadRequestException("Month must be 1,3.6");
        }
    }

    public async Task<string> ProcessPaymentWebhookAsync(WebhookType webhookBody,Guid mentorId)
    {
        PayOS payOs = new PayOS(_clientId, _apiKey, _checksumKey);
        WebhookData webhookData = payOs.verifyPaymentWebhookData(webhookBody);
        if (webhookData.code == "00" && webhookData.desc == "success")
        {
            int monthToAdd = GetMonthsFromDescription(webhookData.description);
            if (monthToAdd > 0)
            {
                var mentor = _mentorRepository.FindByCondition(m => m.mentorDetailsId.Equals(mentorId)).FirstOrDefault();
                if (mentor is null)
                {
                    throw new NotFoundException("Not Found");
                }

                mentor.vipExpirationDate = mentor.vipExpirationDate > DateTime.UtcNow
                    ? mentor.vipExpirationDate.AddMonths(monthToAdd)
                    : DateTime.UtcNow.AddMonths(monthToAdd);
                _mentorRepository.Update(mentor);
                
                if (!(await _mentorRepository.Commit() > 0))
                {
                    throw new BadRequestException("Something went wrong");
                }

            }
        }
        return "Payment and mentor VIP status updated successfully.";
    }
    public async Task<string> ConfirmWebhook(string webhookUrl)
    {
        PayOS payOS = new PayOS(_clientId, _apiKey, _checksumKey);
        return await payOS.confirmWebhook(webhookUrl);
    }
}