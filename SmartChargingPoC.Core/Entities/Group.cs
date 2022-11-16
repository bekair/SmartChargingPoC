using SmartChargingPoC.Core.Entities.Base;

namespace SmartChargingPoC.Core.Entities
{
    public class Group : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double CapacityInAmps { get; set; }

        //Navigation Properties
        public virtual ICollection<ChargeStation> ChargeStations { get; set; }
    }
}