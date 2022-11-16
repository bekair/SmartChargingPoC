using SmartChargingPoC.Core.Constants;
using SmartChargingPoC.Core.Entities;
using Microsoft.EntityFrameworkCore;
using SmartChargingPoC.DataAccess.Contexts;
using SmartChargingPoC.DataAccess.Exceptions;
using SmartChargingPoC.DataAccess.Repositories.Contexts.SmartCharging.Base;
using SmartChargingPoC.DataAccess.Repositories.Contexts.SmartCharging.Implementations.Interfaces;

namespace SmartChargingPoC.DataAccess.Repositories.Contexts.SmartCharging.Implementations
{
    public class ChargeStationRepository : SmartChargingRepositoryBase<ChargeStation>, IChargeStationRepository
    {
        public ChargeStationRepository(SmartChargingContext context)
            : base(context)
        {
        }

        public ChargeStation GetChargeStation(int id)
        {
            return GetIncludeBy(id, chargeStation => chargeStation.Connectors);
        }

        public ICollection<ChargeStation> GetAllChargeStations()
        {
            return _context.ChargeStations
                .Include(x => x.Connectors)
                .ToList();
        }

        public ChargeStation GetChargeStationWithConnectors(int id)
        {
            return GetIncludeBy(id, x => x.Connectors);
        }

        public void CreateChargeStation(ChargeStation newChargeStation)
        {
            Insert(newChargeStation);
        }

        public void UpdateChargeStation(ChargeStation chargeStation)
        {
            Update(chargeStation);
        }

        public void DeleteChargeStation(int id)
        {
            var deletedChargeStation = GetIncludeBy(id, x => x.Connectors);
            Delete(deletedChargeStation);
        }

        public void CheckExistedChargeStation(int chargeStationId, string? errorMessage = null)
        {
            var isChargeStationExist = Any(chargeStationId);
            if (!isChargeStationExist)
                throw new DataNotFoundException(errorMessage ?? string.Format(ApiConstants.ErrorMessage.DataNotFound, ApiConstants.PropertyName.ChargeStation));
        }

        public void CheckConnectorSlotAvailability(int chargeStationId)
        {
            CheckExistedChargeStation(chargeStationId);

            var numberOfConnectorsInChargeStation = _context.ChargeStations
                .Where(x => x.Id == chargeStationId)
                .SelectMany(x => x.Connectors)
                .Count();

            if (numberOfConnectorsInChargeStation >= 5)
                throw new DataNotFoundException(ApiConstants.ErrorMessage.NoConnectorSlotInChargeStation);
        }
    }
}