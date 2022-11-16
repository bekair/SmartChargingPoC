using SmartChargingPoC.Core.Entities.Base;

namespace SmartChargingPoC.Core.Entities
{
    public class ChargeStation : IEntity
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string Name { get; set; }

        //Navigation Properties
        public virtual Group Group { get; set; }
        public virtual ICollection<Connector> Connectors { get; set; }
    }
}