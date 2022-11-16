using SmartChargingPoC.Business.Dtos.Requests.Connectors;
using SmartChargingPoC.Business.Dtos.Responses.Connectors;
using SmartChargingPoC.Core.Entities;

namespace SmartChargingPoC.Business.UnitTests.Mocks;

public static class MockConnectorData
{
    internal static Connector ConnectorData(int connectorId)
    {
        return new Connector
        {
            Id = connectorId,
            ChargeStationId = 1,
            ChargeStationUniqueNumber = 1,
            MaxCurrentInAmps = 10
        };
    }
    internal static ConnectorResponseDto ConnectorResponseDtoData(int connectorId)
    {
        return new ConnectorResponseDto
        {
            Id = connectorId,
            ChargeStationId = 1,
            ChargeStationUniqueNumber = 1,
            MaxCurrentInAmps = 10
        };
    }
    internal static ConnectorResponseDto MapConnectorToConnectorResponseDtoData(Connector connector)
    {
        return new ConnectorResponseDto
        {
            Id = connector.Id,
            ChargeStationId = connector.ChargeStationId,
            ChargeStationUniqueNumber = connector.ChargeStationUniqueNumber,
            MaxCurrentInAmps = connector.MaxCurrentInAmps
        };
    }
    internal static ICollection<Connector> ConnectorCollection(params int[] connectorIdParams)
    {
        return connectorIdParams.Select(ConnectorData).ToList();
    }
    internal static ICollection<ConnectorResponseDto> ConnectorResponseDtoCollection(params int[] connectorIdParams)
    {
        return connectorIdParams.Select(ConnectorResponseDtoData).ToList();
    }
    internal static CreateConnectorRequestDto CreateConnectorRequestDtoData(int chargeStationId, double maxCurrentInAmps)
    {
        return new CreateConnectorRequestDto
        {
            ChargeStationId = chargeStationId,
            MaxCurrentInAmps = maxCurrentInAmps
        };
    }
    internal static ConnectorResponseDto CreateConnectorResponseDtoData(int connectorId, int chargeStationUniqueNumber, CreateConnectorRequestDto createGroupRequestDto)
    {
        return new ConnectorResponseDto
        {
            Id = connectorId,
            ChargeStationId = createGroupRequestDto.ChargeStationId,
            ChargeStationUniqueNumber = chargeStationUniqueNumber,
            MaxCurrentInAmps = createGroupRequestDto.MaxCurrentInAmps
        };
    }
    internal static UpdateConnectorRequestDto UpdateConnectorRequestDtoData(double maxCurrentInAmps)
    {
        return new UpdateConnectorRequestDto
        {
            MaxCurrentInAmps = maxCurrentInAmps
        };
    }
}