using System.ComponentModel.DataAnnotations;
using SmartChargingPoC.Core.Constants;
using SmartChargingPoC.DataAccess.Exceptions;
using SmartChargingPoC.Business.CustomValidators;
using SmartChargingPoC.Business.Dtos.Requests.Base;

namespace SmartChargingPoC.Business.Dtos.Requests.Connectors;

public class CreateConnectorRequestDto : RequestDtoBase
{
    public int ChargeStationId { get; set; }

    [Display(Name = ApiConstants.PropertyName.ConnectorMaxCurrentInAmps)]
    [DoubleGreaterThan(0, ErrorMessage = ApiConstants.ErrorMessage.GreaterThan)]
    public double MaxCurrentInAmps { get; set; }

    public override void CheckModelValidation()
    {
        if (MaxCurrentInAmps <= 0)
            throw new LessThanValueException(string.Format(ApiConstants.ErrorMessage.GreaterThan, ApiConstants.PropertyName.MaxCurrentInAmps, ApiConstants.General.MinCapacityInAmpsValue));
    }
}