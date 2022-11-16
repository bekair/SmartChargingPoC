using SmartChargingPoC.Business.Dtos.Responses.ChargeStations;

namespace SmartChargingPoC.Business.Dtos.Responses.Groups;

public class GroupResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double CapacityInAmps { get; set; }
    public ICollection<ChargeStationResponseDto> ChargeStations { get; set; }
}