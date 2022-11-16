using AutoMapper;
using SmartChargingPoC.Core.Entities;
using SmartChargingPoC.Business.Dtos.Requests.ChargeStations;
using SmartChargingPoC.Business.Dtos.Responses.ChargeStations;

namespace SmartChargingPoC.Business.Profiles;

public class ChargeStationProfile : Profile
{
    public ChargeStationProfile()
    {
        CreateMap<CreateChargeStationRequestDto, ChargeStation>()
            .ForPath(destination =>
                    destination.Name,
                source => source.MapFrom(src => src.ChargeStationName)
            );
        CreateMap<ChargeStation, ChargeStationResponseDto>();
    }
}