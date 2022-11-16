using SmartChargingPoC.Core.Entities;
using SmartChargingPoC.DataAccess.Repositories.Contexts.SmartCharging.Base;

namespace SmartChargingPoC.DataAccess.Repositories.Contexts.SmartCharging.Implementations.Interfaces
{
    public interface IGroupRepository: ISmartChargingRepositoryBase<Group>
    {
        Group GetGroup(int id);
        ICollection<Group> GetAllGroups();
        void CreateGroup(Group newGroup);
        void UpdateGroup(Group group);
        void DeleteGroup(int id);
        bool IsMaxCurrentCapacityAvailable(int groupId, double totalMaxCurrentInConnectors);
        int GetGroupIdByChargeStationId(int chargeStationId);
        int GetGroupIdByConnectorId(int connectorId);
        void CheckExistedGroup(int groupId, string? errorMessage = null);
    }
}
