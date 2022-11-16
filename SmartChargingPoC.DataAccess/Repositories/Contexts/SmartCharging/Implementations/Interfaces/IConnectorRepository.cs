using SmartChargingPoC.Core.Entities;
using SmartChargingPoC.DataAccess.Repositories.Contexts.SmartCharging.Base;

namespace SmartChargingPoC.DataAccess.Repositories.Contexts.SmartCharging.Implementations.Interfaces
{
    public interface IConnectorRepository : ISmartChargingRepositoryBase<Connector>
    {
        Connector GetConnector(int id);
        double GetTotalMaxCurrentByGroupById(int groupId);
        int GetLastConnectorUniqueNumber(int chargeStationId);
        void CreateConnector(Connector newConnector);
        void UpdateConnector(Connector connector);
        void DeleteConnector(int id);
    }
}