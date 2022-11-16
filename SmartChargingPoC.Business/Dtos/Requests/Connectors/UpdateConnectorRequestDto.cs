using System.ComponentModel.DataAnnotations;
using SmartChargingPoC.Core.Constants;
using SmartChargingPoC.Business.CustomValidators;

namespace SmartChargingPoC.Business.Dtos.Requests.Connectors;

public class UpdateConnectorRequestDto
{
    [Display(Name = ApiConstants.PropertyName.ConnectorMaxCurrentInAmps)]
    [Required(ErrorMessage = ApiConstants.ErrorMessage.Required)]
    [DoubleGreaterThan(0, ErrorMessage = ApiConstants.ErrorMessage.GreaterThan)]
    public double MaxCurrentInAmps { get; set; }
}