using SmartChargingPoC.Business.Dtos.Requests.ChargeStations;
using SmartChargingPoC.Business.Dtos.Responses.ChargeStations;

namespace SmartChargingPoC.Business.Services.Interfaces;

public interface IChargeStationService
{
    ChargeStationResponseDto GetChargeStation(int id);
    ICollection<ChargeStationResponseDto> GetAllChargeStations();
    ChargeStationResponseDto CreateChargeStation(CreateChargeStationRequestDto createChargeStationRequestDto);
    void UpdateChargeStation(int id, UpdateChargeStationRequestDto updateChargeStationRequestDto);
    void DeleteChargeStation(int id);
}