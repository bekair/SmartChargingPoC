using AutoMapper;
using SmartChargingPoC.Core.Entities;
using SmartChargingPoC.Business.Dtos.Requests.Groups;
using SmartChargingPoC.Business.Dtos.Responses.Groups;

namespace SmartChargingPoC.Business.Profiles;

public class GroupProfile : Profile
{
    public GroupProfile()
    {
        CreateMap<Group, GroupResponseDto>();
        CreateMap<CreateGroupRequestDto, Group>();
        CreateMap<UpdateGroupRequestDto, Group>();
        CreateMap<Group, CreateGroupResponseDto>();
    }
}