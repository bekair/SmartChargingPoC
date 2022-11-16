using AutoMapper;
using SmartChargingPoC.Core.Constants;
using SmartChargingPoC.Core.Entities;
using SmartChargingPoC.DataAccess.Exceptions;
using SmartChargingPoC.DataAccess.UnitOfWorks.Base;
using SmartChargingPoC.Business.Dtos.Requests.Groups;
using SmartChargingPoC.Business.Dtos.Responses.Groups;
using SmartChargingPoC.Business.Services.Base;
using SmartChargingPoC.Business.Services.Interfaces;

namespace SmartChargingPoC.Business.Services;

public class GroupService : ServiceBase, IGroupService
{
    public GroupService(IUnitOfWork unitOfWork, IMapper mapper)
        : base(unitOfWork, mapper)
    {
    }

    public GroupResponseDto GetGroup(int id)
    {
        var selectedGroup = _unitOfWork.GroupRepository.GetGroup(id);

        return _mapper.Map<GroupResponseDto>(selectedGroup);
    }

    public ICollection<GroupResponseDto> GetAllGroups()
    {
        var groups = _unitOfWork.GroupRepository.GetAllGroups();
        
        return _mapper.Map<ICollection<GroupResponseDto>>(groups);
    }
    
    public CreateGroupResponseDto CreateGroup(CreateGroupRequestDto createGroupRequestDto)
    {
        if (createGroupRequestDto is null)
            throw new ArgumentNullException(nameof(createGroupRequestDto), string.Format(ApiConstants.ErrorMessage.Required, ApiConstants.General.RequestedParameter));
        
        createGroupRequestDto.CheckModelValidation();
        
        var createdGroup = _mapper.Map<Group>(createGroupRequestDto);
        _unitOfWork.GroupRepository.CreateGroup(createdGroup);
        _unitOfWork.Commit();

        return _mapper.Map<CreateGroupResponseDto>(createdGroup);
    }

    public void UpdateGroup(int id, UpdateGroupRequestDto updateGroupRequestDto)
    {
        if (updateGroupRequestDto is null)
            throw new ArgumentNullException(nameof(updateGroupRequestDto), string.Format(ApiConstants.ErrorMessage.Required, ApiConstants.General.RequestedParameter));
        
        updateGroupRequestDto.CheckModelValidation();
        
        var updatedGroup = _unitOfWork.GroupRepository.GetGroup(id);
        _mapper.Map(updateGroupRequestDto, updatedGroup);
        
        _unitOfWork.GroupRepository.UpdateGroup(updatedGroup);
        _unitOfWork.Commit();
    }

    public void DeleteGroup(int id)
    {
        _unitOfWork.GroupRepository.DeleteGroup(id);
        _unitOfWork.Commit();
    }

    public void CheckMaxCurrentCapacityExcess(int groupId, double newMaxCurrentAdded)
    {
        var totalMaxCurrentOfConnectors = _unitOfWork.ConnectorRepository.GetTotalMaxCurrentByGroupById(groupId);
        var updatedTotalMaxCurrent = totalMaxCurrentOfConnectors + newMaxCurrentAdded;
        
        var isCapacityAvailable = _unitOfWork.GroupRepository.IsMaxCurrentCapacityAvailable(groupId, updatedTotalMaxCurrent);
        if (!isCapacityAvailable)
            throw new ExceededCurrentCapacityException(ApiConstants.ErrorMessage.ExceededCurrentCapacity);
    }
}