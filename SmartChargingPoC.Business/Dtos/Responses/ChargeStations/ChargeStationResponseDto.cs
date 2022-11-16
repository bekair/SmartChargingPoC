using SmartChargingPoC.Business.Dtos.Responses.Connectors;

namespace SmartChargingPoC.Business.Dtos.Responses.ChargeStations;

public class ChargeStationResponseDto
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public string Name { get; set; }
    public ICollection<ConnectorResponseDto> Connectors { get; set; }
}