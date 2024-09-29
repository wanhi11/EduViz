using AutoMapper;
using EduViz.Dtos;
using EduViz.Entities;
using EduViz.Enums;
using EduViz.Exceptions;
using EduViz.Repositories;
using Microsoft.VisualBasic;

namespace EduViz.Services;

public class UpgradeOrderDetailService
{
    private readonly IRepository<UpgradeOrderDetails, Guid> _upgradeOrderRepository;
    private readonly IMapper _mapper;
    private readonly IRepository<MentorDetails, Guid> _mentorDetailsRepository;

    public UpgradeOrderDetailService(IRepository<UpgradeOrderDetails, Guid> upgradeOrderRepository, IMapper mapper,
        IRepository<MentorDetails,Guid> mentorDetailsRepository)
    {
        _upgradeOrderRepository = upgradeOrderRepository;
        _mapper = mapper;
        _mentorDetailsRepository = mentorDetailsRepository;
    }

    public UpgradeOrderDetailModel? FindOrderByCode(long code)
    {
        var result = _upgradeOrderRepository.FindByCondition(
            o => o.orderCode == code).FirstOrDefault();
        if (result is null)
        {
            return null;
        }

        return _mapper.Map<UpgradeOrderDetailModel>(result);
    }

    public async Task UpdateStatus(long code, PaymentStatus status)
    {
        var order = _upgradeOrderRepository.FindByCondition(
            o => o.orderCode == code).FirstOrDefault();
        if (order is null)
        {
            throw new NotFoundException("Upgrade Order not found");
        }

        order.paymentStatus = status;
        _upgradeOrderRepository.Update(order);
        if (!(await _upgradeOrderRepository.Commit() >0))
        {
            throw new BadRequestException("Something went wrong when update order");
        }

        var mentor = _mentorDetailsRepository.FindByCondition(m => m.mentorDetailsId.Equals(order.mentorDetailsID))
            .First();
        int month = (int)order.packageName[0];
        mentor.vipExpirationDate = DateTime.Now.AddMonths(month);
        _mentorDetailsRepository.Update(mentor);
        if (!(await _mentorDetailsRepository.Commit() > 0))
        {
            throw new BadRequestException("Something went wrong when upgrade vip");
        }
    }

}   