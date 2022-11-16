using SmartChargingPoC.DataAccess.Repositories.Contexts.SmartCharging.Implementations.Interfaces;

namespace SmartChargingPoC.DataAccess.UnitOfWorks.Base
{
    public interface IUnitOfWork : IDisposable
    {
        IGroupRepository GroupRepository { get; }
        IConnectorRepository ConnectorRepository { get; }
        IChargeStationRepository ChargeStationRepository { get; }
        int Commit();
    }
}
