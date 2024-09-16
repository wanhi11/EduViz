using AutoMapper;
using EduViz.Dtos;
using EduViz.Entities;
using EduViz.Enums;
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
        payment.PaymentDate = DateTime.Now;
        payment.PaymentStatus = PaymentStatus.Pending;

        payment = await _paymentRepository.AddAsync(payment);
        if (await _paymentRepository.Commit() > 0)
        {
            return newModel;
        }

        return null;
    }
}