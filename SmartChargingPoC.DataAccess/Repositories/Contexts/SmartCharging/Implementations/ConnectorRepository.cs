using SmartChargingPoC.Core.Constants;
using SmartChargingPoC.Core.Entities;
using SmartChargingPoC.DataAccess.Contexts;
using SmartChargingPoC.DataAccess.Repositories.Contexts.SmartCharging.Base;
using SmartChargingPoC.DataAccess.Repositories.Contexts.SmartCharging.Implementations.Interfaces;

namespace SmartChargingPoC.DataAccess.Repositories.Contexts.SmartCharging.Implementations
{
    public class ConnectorRepository : SmartChargingRepositoryBase<Connector>, IConnectorRepository
    {
        public ConnectorRepository(SmartChargingContext context)
            : base(context)
        {
        }

        public Connector GetConnector(int id)
        {
            return GetById(id);
        }

        public double GetTotalMaxCurrentByGroupById(int groupId)
        {
            return _context.Groups
                .Where(x => x.Id == groupId)
                .SelectMany(x => x.ChargeStations)
                .SelectMany(x => x.Connectors)
                .Sum(x => x.MaxCurrentInAmps);
        }

        public int GetLastConnectorUniqueNumber(int chargeStationId)
        {
            var lastConnectorUniqueNumber = _context.Connectors
                .Where(x => x.ChargeStationId == chargeStationId)
                .OrderByDescending(x => x.ChargeStationUniqueNumber)
                .Select(x => x.ChargeStationUniqueNumber)
                .FirstOrDefault();

            if (lastConnectorUniqueNumber == 0)
                throw new ArgumentNullException(nameof(lastConnectorUniqueNumber), string.Format(ApiConstants.ErrorMessage.NotExistedLastConnectorUniqueNumber, chargeStationId));

            return lastConnectorUniqueNumber;
        }

        public void CreateConnector(Connector newConnector)
        {
            Insert(newConnector);
        }

        public void UpdateConnector(Connector connector)
        {
            Update(connector);
        }

        public void DeleteConnector(int id)
        {
            var deletedConnector = GetById(id);
            Delete(deletedConnector);
        }
    }
}