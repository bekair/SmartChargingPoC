using System.ComponentModel.DataAnnotations;
using SmartChargingPoC.Core.Constants;
using SmartChargingPoC.DataAccess.Exceptions;
using SmartChargingPoC.Business.CustomValidators;
using SmartChargingPoC.Business.Dtos.Requests.Base;

namespace SmartChargingPoC.Business.Dtos.Requests.Groups;

public class UpdateGroupRequestDto:RequestDtoBase
{
    [Display(Name = ApiConstants.PropertyName.Name)]
    [Required(ErrorMessage = ApiConstants.ErrorMessage.Required)]
    public string Name { get; set; }
    
    [Display(Name = ApiConstants.PropertyName.CapacityInAmps)]
    [DoubleGreaterThan(0, ErrorMessage = ApiConstants.ErrorMessage.GreaterThan)]
    public double CapacityInAmps { get; set; }

    public override void CheckModelValidation()
    {
        if (Name is null)
            throw new ArgumentNullException(nameof(Name), string.Format(ApiConstants.ErrorMessage.Required, ApiConstants.PropertyName.Name));

        if (CapacityInAmps <= 0)
            throw new LessThanValueException(string.Format(ApiConstants.ErrorMessage.GreaterThan, ApiConstants.PropertyName.CapacityInAmps, ApiConstants.General.MinCapacityInAmpsValue));
    }
}