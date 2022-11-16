using System.ComponentModel.DataAnnotations;
using SmartChargingPoC.Core.Constants;

namespace SmartChargingPoC.Business.CustomValidators;

public class DoubleGreaterThan : ValidationAttribute
{
    public double CompareNumber { get; }
    
    public DoubleGreaterThan(double compareNumber)
    {
        CompareNumber = compareNumber;
    }

    protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
    {
        if (value is not double currentValue)
            throw new InvalidOperationException(string.Format(ApiConstants.ErrorMessage.InvalidPropertyType, nameof(Double)));

        if (!(currentValue <= CompareNumber)) 
            return ValidationResult.Success;
        
        ErrorMessage = string.Format(ErrorMessageString, validationContext.MemberName, CompareNumber);
        return new ValidationResult(ErrorMessage);

    }
}

