using System.ComponentModel.DataAnnotations;
using SmartChargingPoC.Core.Constants;
using SmartChargingPoC.Core.Entities.Base;

namespace SmartChargingPoC.Core.Entities
{
    public class Connector : IEntity
    {
        public int Id { get; set; }

        public int ChargeStationId { get; set; }

        [Display(Name = ApiConstants.PropertyName.ChargeStationUniqueNumber)]
        [Range(1, 5, ErrorMessage = ApiConstants.ErrorMessage.Range)]
        public int ChargeStationUniqueNumber { get; set; }

        public double MaxCurrentInAmps { get; set; }

        //Navigation Properties
        public virtual ChargeStation ChargeStation { get; set; }
    }
}