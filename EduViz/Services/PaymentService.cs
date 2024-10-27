using AutoMapper;
using EduViz.Common.Payloads.Response;
using EduViz.Dtos;
using EduViz.Entities;
using EduViz.Enums;
using EduViz.Exceptions;
using EduViz.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EduViz.Services;

public class PaymentService
{
    private readonly IRepository<Payment, Guid> _paymentRepository;
    private readonly IMapper _mapper;
    private readonly IRepository<UpgradeOrderDetails, Guid> _upgradeRepository;


    public PaymentService(IRepository<Payment,Guid> paymentRepository,IMapper mapper,IRepository<UpgradeOrderDetails,Guid> upgradeRepository)
    {
        _paymentRepository = paymentRepository;
        _mapper = mapper;
        _upgradeRepository = upgradeRepository;
    }

    public async Task<PaymentModel> CreateNewPayment(PaymentModel newModel)
    {
        var payment = _mapper.Map<Payment>(newModel);
        payment.paymentDate = DateTime.Now;
        payment.paymentStatus = PaymentStatus.Pending;

        payment = await _paymentRepository.AddAsync(payment);
        if (await _paymentRepository.Commit() > 0)
        {
            return newModel;
        }

        return null;
    }

    public PaymentModel? GetPaymentById(Guid code)
    {
        var result = _paymentRepository.FindByCondition(p => p.paymentId.Equals(code)).FirstOrDefault();
        if (result is null) return null;
        return _mapper.Map<PaymentModel>(result);
    }

    public async Task UpdateStatus(Guid paymentId, PaymentStatus status)
    {
        var payment = _paymentRepository.FindByCondition(p => p.paymentId.Equals(paymentId)).FirstOrDefault();
        if (payment is null)
        {
            throw new NotFoundException("course payment does not found");
        }

        payment.paymentStatus = status;
        _paymentRepository.Update(payment);
        if (!(await _paymentRepository.Commit() > 0))
        {
            throw new BadRequestException("Something went wrong when update payment");
        }
    }

    public  List<AnalysPayment> GetAllPaymentById(Guid mentorId)
    {
        var payment = _paymentRepository.FindByCondition(p => p.mentorId.Equals(mentorId) && p.paymentStatus.ToString().Equals("Completed"))
            .Include(p => p.student)
            .Include(p => p.course)
            .OrderBy(p => p.paymentDate )
            .Select(p=>new AnalysPayment()
            {
                paymentId = p.paymentId,
                Amount =(int) p.amount,
                courseName = p.course.courseName,
                studentName = p.student.userName,
                PaymentDate = p.paymentDate
            }).ToList();
        return payment;
    }

    public List<UpgradeAnalysis> GetUpdateAnalys()
    {
        var analys = _upgradeRepository.FindByCondition(v => v.paymentStatus.ToString().Equals("Completed"))
            .Include(v => v.mentorDetails)
            .ThenInclude(m => m.user)
            .Select(v => new UpgradeAnalysis()
            {
                upgradeOrderDetailsId = v.upgradeOrderDetailsId,
                amount = v.amount,
                mentorName = v.mentorDetails.user.userName!,
                packageName = v.packageName,
                orderCode = v.orderCode,
                UpgradeDate = v.UpgradeDate
            }).ToList();
        return analys;
    }
}