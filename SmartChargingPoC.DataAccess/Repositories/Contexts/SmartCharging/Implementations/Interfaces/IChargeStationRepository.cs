using SmartChargingPoC.Core.Entities;
using SmartChargingPoC.DataAccess.Repositories.Contexts.SmartCharging.Base;

namespace SmartChargingPoC.DataAccess.Repositories.Contexts.SmartCharging.Implementations.Interfaces
{
    public interface IChargeStationRepository : ISmartChargingRepositoryBase<ChargeStation>
    {
        ChargeStation GetChargeStation(int id);
        ICollection<ChargeStation> GetAllChargeStations();
        void CreateChargeStation(ChargeStation newChargeStation);
        void UpdateChargeStation(ChargeStation chargeStation);
        void DeleteChargeStation(int id);
        void CheckExistedChargeStation(int chargeStationId, string? errorMessage = null);
        void CheckConnectorSlotAvailability(int chargeStationId);
    }
}