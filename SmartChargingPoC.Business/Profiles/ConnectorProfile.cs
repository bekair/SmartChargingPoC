using AutoMapper;
using SmartChargingPoC.Core.Entities;
using SmartChargingPoC.Business.Dtos.Requests.Connectors;
using SmartChargingPoC.Business.Dtos.Responses.Connectors;

namespace SmartChargingPoC.Business.Profiles;

public class ConnectorProfile : Profile
{
    public ConnectorProfile()
    {
        CreateMap<Connector, ConnectorResponseDto>();
        CreateMap<CreateConnectorRequestDto, Connector>();
    }
}