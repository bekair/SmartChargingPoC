namespace SmartChargingPoC.Business.Dtos.Requests.Base;

public abstract class RequestDtoBase : IRequestDtoBase
{
    public abstract void CheckModelValidation();
}