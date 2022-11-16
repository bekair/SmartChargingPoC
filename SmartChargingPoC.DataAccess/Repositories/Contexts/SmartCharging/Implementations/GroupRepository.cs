using SmartChargingPoC.Core.Constants;
using SmartChargingPoC.Core.Entities;
using Microsoft.EntityFrameworkCore;
using SmartChargingPoC.DataAccess.Contexts;
using SmartChargingPoC.DataAccess.Exceptions;
using SmartChargingPoC.DataAccess.Repositories.Contexts.SmartCharging.Base;
using SmartChargingPoC.DataAccess.Repositories.Contexts.SmartCharging.Implementations.Interfaces;

namespace SmartChargingPoC.DataAccess.Repositories.Contexts.SmartCharging.Implementations
{
    public class GroupRepository : SmartChargingRepositoryBase<Group>, IGroupRepository
    {
        public GroupRepository(SmartChargingContext context)
            : base(context)
        {
        }

        public Group GetGroup(int id)
        {
            var group = _context.Groups
                .Where(x=>x.Id == id)
                .Include(x=>x.ChargeStations)
                    .ThenInclude(x=>x.Connectors)
                .FirstOrDefault();

            if (group is null)
                throw new DataNotFoundException(string.Format(ApiConstants.ErrorMessage.DataNotFound, ApiConstants.PropertyName.Group));

            return group;
        }

        public ICollection<Group> GetAllGroups()
        {
            return _context.Groups
                .Include(x=> x.ChargeStations)
                    .ThenInclude(x=> x.Connectors)
                .ToList();
        }

        public void CreateGroup(Group newGroup)
        {
            Insert(newGroup);
        }

        public void UpdateGroup(Group group)
        {
            Update(group);
        }

        public void DeleteGroup(int id)
        {
            var deletedGroup = _context.Groups
                .Where(x=>x.Id == id)
                .Include(x=>x.ChargeStations)
                    .ThenInclude(x=>x.Connectors)
                .FirstOrDefault();
            
            if (deletedGroup is null)
                throw new DataNotFoundException(string.Format(ApiConstants.ErrorMessage.DataNotFound, ApiConstants.PropertyName.Group));
            
            Delete(deletedGroup);
        }

        public bool IsMaxCurrentCapacityAvailable(int groupId, double newTotalMaxCurrentInConnectors)
        {
            var group = GetById(groupId);
            return group.CapacityInAmps >= newTotalMaxCurrentInConnectors;
        }

        public int GetGroupIdByChargeStationId(int chargeStationId)
        {
            var groupId = _context.ChargeStations
                .Where(x => x.Id == chargeStationId)
                .Select(x => x.Group.Id)
                .FirstOrDefault();
            
            if(groupId == 0)
                throw new ArgumentNullException(nameof(groupId), ApiConstants.ErrorMessage.NotExistedGroupIdByChargeStationId);

            return groupId;
        }

        public int GetGroupIdByConnectorId(int connectorId)
        {
            var groupId = _context.Connectors
                .Where(x => x.Id == connectorId)
                .Select(x => x.ChargeStation.GroupId)
                .FirstOrDefault();
            
            if(groupId == 0)
                throw new ArgumentNullException(nameof(groupId), ApiConstants.ErrorMessage.NotExistedGroupIdByConnectorId);

            return groupId;
        }

        public void CheckExistedGroup(int groupId, string? errorMessage = null)
        {
            var isGroupStationExist = Any(groupId);
            if (!isGroupStationExist)
                throw new DataNotFoundException(errorMessage ?? string.Format(ApiConstants.ErrorMessage.DataNotFound, ApiConstants.PropertyName.Group));
        }
    }
}