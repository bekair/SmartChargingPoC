namespace SmartChargingPoC.Business.Dtos.Responses.Connectors;

public class ConnectorResponseDto
{
    public int Id { get; set; }
    public int ChargeStationId { get; set; }
    public int ChargeStationUniqueNumber { get; set; }
    public double MaxCurrentInAmps { get; set; }
}