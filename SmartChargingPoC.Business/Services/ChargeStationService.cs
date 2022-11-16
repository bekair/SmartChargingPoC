using AutoMapper;
using SmartChargingPoC.Core.Constants;
using SmartChargingPoC.Core.Entities;
using SmartChargingPoC.DataAccess.UnitOfWorks.Base;
using SmartChargingPoC.Business.Dtos.Requests.ChargeStations;
using SmartChargingPoC.Business.Dtos.Responses.ChargeStations;
using SmartChargingPoC.Business.Services.Base;
using SmartChargingPoC.Business.Services.Interfaces;

namespace SmartChargingPoC.Business.Services;

public class ChargeStationService : ServiceBase, IChargeStationService
{
    private readonly IGroupService _groupService;
    
    public ChargeStationService(IUnitOfWork unitOfWork, IMapper mapper, IGroupService groupService)
        : base(unitOfWork, mapper)
    {
        _groupService = groupService;
    }

    public ChargeStationResponseDto GetChargeStation(int id)
    {
        var selectedChargeStation = _unitOfWork.ChargeStationRepository.GetChargeStation(id);

        return _mapper.Map<ChargeStationResponseDto>(selectedChargeStation);
    }

    public ICollection<ChargeStationResponseDto> GetAllChargeStations()
    {
        var chargeStations = _unitOfWork.ChargeStationRepository.GetAllChargeStations();
        
        return _mapper.Map<ICollection<ChargeStationResponseDto>>(chargeStations);
    }

    public ChargeStationResponseDto CreateChargeStation(CreateChargeStationRequestDto createChargeStationRequestDto)
    {
        _unitOfWork.GroupRepository.CheckExistedGroup(createChargeStationRequestDto.GroupId, ApiConstants.ErrorMessage.NotExistedGroupChargeStationOperation);

        _groupService.CheckMaxCurrentCapacityExcess(createChargeStationRequestDto.GroupId, createChargeStationRequestDto.ConnectorMaxCurrentInAmps);

        var newChargeStation = _mapper.Map<ChargeStation>(createChargeStationRequestDto);
        newChargeStation.Connectors = new List<Connector>
        {
            new()
            {
                ChargeStationUniqueNumber = 1,
                MaxCurrentInAmps = createChargeStationRequestDto.ConnectorMaxCurrentInAmps
            }
        };

        _unitOfWork.ChargeStationRepository.CreateChargeStation(newChargeStation);
        _unitOfWork.Commit();
        
        return _mapper.Map<ChargeStationResponseDto>(newChargeStation);
    }

    public void UpdateChargeStation(int id, UpdateChargeStationRequestDto updateChargeStationRequestDto)
    {
        var updatedChargeStation = _unitOfWork.ChargeStationRepository.GetChargeStation(id);
        updatedChargeStation.Name = updateChargeStationRequestDto.Name;
        
        _unitOfWork.ChargeStationRepository.UpdateChargeStation(updatedChargeStation);
        _unitOfWork.Commit();
    }

    public void DeleteChargeStation(int id)
    {
        _unitOfWork.ChargeStationRepository.DeleteChargeStation(id);
        _unitOfWork.Commit();
    }
}