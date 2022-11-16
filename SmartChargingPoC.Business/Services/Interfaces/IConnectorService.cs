using SmartChargingPoC.Business.Dtos.Requests.Connectors;
using SmartChargingPoC.Business.Dtos.Responses.Connectors;

namespace SmartChargingPoC.Business.Services.Interfaces;

public interface IConnectorService
{
    ConnectorResponseDto GetConnector(int id);
    ICollection<ConnectorResponseDto> GetAllConnectors();
    ConnectorResponseDto CreateConnector(CreateConnectorRequestDto createConnectorRequestDto);
    void UpdateConnector(int id, UpdateConnectorRequestDto updateConnectorRequestDto);
    void DeleteConnector(int id);
}