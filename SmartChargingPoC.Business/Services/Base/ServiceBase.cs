using AutoMapper;
using SmartChargingPoC.DataAccess.UnitOfWorks.Base;

namespace SmartChargingPoC.Business.Services.Base;

public class ServiceBase
{
    protected readonly IUnitOfWork _unitOfWork;
    protected readonly IMapper _mapper;

    protected ServiceBase(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
}