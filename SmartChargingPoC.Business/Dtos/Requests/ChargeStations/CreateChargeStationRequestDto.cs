using System.ComponentModel.DataAnnotations;
using SmartChargingPoC.Core.Constants;
using SmartChargingPoC.Business.CustomValidators;

namespace SmartChargingPoC.Business.Dtos.Requests.ChargeStations;

public class CreateChargeStationRequestDto
{
    public int GroupId { get; set; }
    
    [Display(Name = ApiConstants.PropertyName.ChargeStation)]
    [Required(ErrorMessage = ApiConstants.ErrorMessage.Required)]
    public string ChargeStationName { get; set; }
    
    [Display(Name = ApiConstants.PropertyName.ConnectorMaxCurrentInAmps)]
    [Required(ErrorMessage = ApiConstants.ErrorMessage.Required)]
    [DoubleGreaterThan(0, ErrorMessage = ApiConstants.ErrorMessage.GreaterThan)]
    public double ConnectorMaxCurrentInAmps { get; set; }
}