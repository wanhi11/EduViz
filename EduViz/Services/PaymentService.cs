using AutoMapper;
using EduViz.Dtos;
using EduViz.Entities;
using EduViz.Enums;
using EduViz.Exceptions;
using EduViz.Repositories;

namespace EduViz.Services;

public class PaymentService
{
    private readonly IRepository<Payment, Guid> _paymentRepository;
    private readonly IMapper _mapper;

    public PaymentService(IRepository<Payment,Guid> paymentRepository,IMapper mapper)
    {
        _paymentRepository = paymentRepository;
        _mapper = mapper;
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
}