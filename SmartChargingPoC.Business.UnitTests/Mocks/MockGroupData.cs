using SmartChargingPoC.Business.Dtos.Requests.Groups;
using SmartChargingPoC.Business.Dtos.Responses.ChargeStations;
using SmartChargingPoC.Business.Dtos.Responses.Connectors;
using SmartChargingPoC.Business.Dtos.Responses.Groups;
using SmartChargingPoC.Core.Entities;

namespace SmartChargingPoC.Business.UnitTests.Mocks;

internal static class MockGroupData
{
    internal static Group GroupData(int groupId)
    {
        return new Group
        {
            Id = groupId,
            Name = "Group 1",
            CapacityInAmps = 100.3,
            ChargeStations = new List<ChargeStation>
            {
                new ChargeStation
                {
                    Id = 1,
                    Name = "Charge Station 1.1",
                    GroupId = groupId,
                    Connectors = new List<Connector>
                    {
                        new Connector
                        {
                            Id = 1,
                            ChargeStationId = 1,
                            ChargeStationUniqueNumber = 1,
                            MaxCurrentInAmps = 25
                        },
                        new Connector
                        {
                            Id = 2,
                            ChargeStationId = 1,
                            ChargeStationUniqueNumber = 2,
                            MaxCurrentInAmps = 43
                        }
                    }
                }
            }
        };
    }

    internal static GroupResponseDto GroupResponseDtoData(int groupId)
    {
        return new GroupResponseDto
        {
            Id = groupId,
            Name = "Group 1",
            CapacityInAmps = 100.3,
            ChargeStations = new List<ChargeStationResponseDto>
            {
                new ChargeStationResponseDto
                {
                    Id = 1,
                    Name = "Charge Station 1.1",
                    GroupId = groupId,
                    Connectors = new List<ConnectorResponseDto>
                    {
                        new ConnectorResponseDto
                        {
                            Id = 1,
                            ChargeStationId = 1,
                            ChargeStationUniqueNumber = 1,
                            MaxCurrentInAmps = 25
                        },
                        new ConnectorResponseDto
                        {
                            Id = 2,
                            ChargeStationId = 1,
                            ChargeStationUniqueNumber = 2,
                            MaxCurrentInAmps = 43
                        }
                    }
                }
            }
        };
    }

    internal static ICollection<Group> GroupCollection(params int[] groupIdParams)
    {
        return groupIdParams.Select(GroupData).ToList();
    }
    
    internal static ICollection<GroupResponseDto> GroupResponseDtoCollection(params int[] groupIdParams)
    {
        return groupIdParams.Select(GroupResponseDtoData).ToList();
    }

    internal static CreateGroupRequestDto CreateGroupRequestDtoData(string name, double capacity)
    {
        return new CreateGroupRequestDto
        {
            Name = name,
            CapacityInAmps = capacity
        };
    }

    internal static CreateGroupResponseDto CreateGroupResponseDtoData(int groupId, CreateGroupRequestDto createGroupRequestDto)
    {
        return new CreateGroupResponseDto
        {
            Id = groupId,
            Name = createGroupRequestDto.Name,
            CapacityInAmps = createGroupRequestDto.CapacityInAmps
        };
    }

    internal static Group MapCreateRequestDtoToGroup(CreateGroupRequestDto createGroupRequestDto)
    {
        return new Group
        {
            Id = new Random().Next(),
            Name = createGroupRequestDto.Name,
            CapacityInAmps = createGroupRequestDto.CapacityInAmps
        };
    }
    
    internal static UpdateGroupRequestDto UpdateGroupRequestDtoData(string name, double capacity)
    {
        return new UpdateGroupRequestDto
        {
            Name = name,
            CapacityInAmps = capacity
        };
    }
}