using SmartChargingPoC.Business.Dtos.Requests.Groups;
using SmartChargingPoC.Business.Dtos.Responses.Groups;

namespace SmartChargingPoC.Business.Services.Interfaces;

public interface IGroupService
{
    GroupResponseDto GetGroup(int id);
    ICollection<GroupResponseDto> GetAllGroups();
    CreateGroupResponseDto CreateGroup(CreateGroupRequestDto createGroupRequestDto);
    void UpdateGroup(int id, UpdateGroupRequestDto updateGroupRequestDto);
    void DeleteGroup(int id);
    void CheckMaxCurrentCapacityExcess(int groupId, double newMaxCurrentAdded);
}