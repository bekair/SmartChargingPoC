using System.ComponentModel.DataAnnotations;
using SmartChargingPoC.Core.Constants;

namespace SmartChargingPoC.Business.Dtos.Requests.ChargeStations;

public class UpdateChargeStationRequestDto
{
    [Display(Name = ApiConstants.PropertyName.Name)]
    [Required(ErrorMessage = ApiConstants.ErrorMessage.Required)]
    public string Name { get; set; }
}