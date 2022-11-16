namespace SmartChargingPoC.DataAccess.Exceptions;

public class LessThanValueException : Exception
{
    public LessThanValueException(string message)
        : base(message)
    {
    }
}