using AutoMapper;
using SmartChargingPoC.Core.Constants;
using SmartChargingPoC.Core.Entities;
using SmartChargingPoC.DataAccess.UnitOfWorks.Base;
using SmartChargingPoC.Business.Dtos.Requests.Connectors;
using SmartChargingPoC.Business.Dtos.Responses.Connectors;
using SmartChargingPoC.Business.Services.Base;
using SmartChargingPoC.Business.Services.Interfaces;

namespace SmartChargingPoC.Business.Services;

public class ConnectorService : ServiceBase, IConnectorService
{
    private readonly IGroupService _groupService;
    
    public ConnectorService(IUnitOfWork unitOfWork, IMapper mapper, IGroupService groupService)
        : base(unitOfWork, mapper)
    {
        _groupService = groupService;
    }

    public ConnectorResponseDto GetConnector(int id)
    {
        var selectedConnector = _unitOfWork.ConnectorRepository.GetConnector(id);

        return _mapper.Map<ConnectorResponseDto>(selectedConnector);
    }

    public ICollection<ConnectorResponseDto> GetAllConnectors()
    {
        var connectors = _unitOfWork.ConnectorRepository.Get();
        
        return _mapper.Map<ICollection<ConnectorResponseDto>>(connectors);
    }

    public ConnectorResponseDto CreateConnector(CreateConnectorRequestDto createConnectorRequestDto)
    {
        if (createConnectorRequestDto is null)
            throw new ArgumentNullException(nameof(createConnectorRequestDto), string.Format(ApiConstants.ErrorMessage.Required, ApiConstants.General.RequestedParameter));
        
        createConnectorRequestDto.CheckModelValidation();
        
        _unitOfWork.ChargeStationRepository.CheckExistedChargeStation(createConnectorRequestDto.ChargeStationId, ApiConstants.ErrorMessage.NotExistedChargeStationInConnectorOperation);
        _unitOfWork.ChargeStationRepository.CheckConnectorSlotAvailability(createConnectorRequestDto.ChargeStationId);
        
        var groupId = _unitOfWork.GroupRepository.GetGroupIdByChargeStationId(createConnectorRequestDto.ChargeStationId);
        _groupService.CheckMaxCurrentCapacityExcess(groupId, createConnectorRequestDto.MaxCurrentInAmps);

        var lastConnectorUniqueNumber = _unitOfWork.ConnectorRepository.GetLastConnectorUniqueNumber(createConnectorRequestDto.ChargeStationId);
        var newConnector = _mapper.Map<Connector>(createConnectorRequestDto);
        newConnector.ChargeStationUniqueNumber = lastConnectorUniqueNumber + 1;

        _unitOfWork.ConnectorRepository.CreateConnector(newConnector);
        _unitOfWork.Commit();
        
        return _mapper.Map<ConnectorResponseDto>(newConnector);
    }

    public void UpdateConnector(int id, UpdateConnectorRequestDto updateConnectorRequestDto)
    {
        var updatedConnector = _unitOfWork.ConnectorRepository.GetConnector(id);
        var groupId = _unitOfWork.GroupRepository.GetGroupIdByConnectorId(id);

        var deltaAddedMaxCurrent = updateConnectorRequestDto.MaxCurrentInAmps - updatedConnector.MaxCurrentInAmps; 
        _groupService.CheckMaxCurrentCapacityExcess(groupId, deltaAddedMaxCurrent);
        
        updatedConnector.MaxCurrentInAmps = updateConnectorRequestDto.MaxCurrentInAmps;
        
        _unitOfWork.ConnectorRepository.UpdateConnector(updatedConnector);
        _unitOfWork.Commit();
    }

    public void DeleteConnector(int id)
    {
        _unitOfWork.ConnectorRepository.DeleteConnector(id);
        _unitOfWork.Commit();
    }
}