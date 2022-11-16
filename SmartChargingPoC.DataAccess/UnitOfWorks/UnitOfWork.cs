using SmartChargingPoC.DataAccess.Contexts;
using SmartChargingPoC.DataAccess.Repositories.Contexts.SmartCharging.Implementations.Interfaces;
using SmartChargingPoC.DataAccess.UnitOfWorks.Base;

namespace SmartChargingPoC.DataAccess.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SmartChargingContext _context;
        public IGroupRepository GroupRepository { get; }
        public IConnectorRepository ConnectorRepository { get; }
        public IChargeStationRepository ChargeStationRepository { get; }

        public UnitOfWork(SmartChargingContext context,
            IGroupRepository groupRepository,
            IConnectorRepository connectorRepository,
            IChargeStationRepository chargeStationRepository)
        {
            _context = context;
            GroupRepository = groupRepository;
            ConnectorRepository = connectorRepository;
            ChargeStationRepository = chargeStationRepository;
        }

        public int Commit()
        {
            return _context.SaveChanges();
        }

        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
