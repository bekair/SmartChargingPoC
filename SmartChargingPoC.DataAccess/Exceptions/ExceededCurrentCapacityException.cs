namespace SmartChargingPoC.DataAccess.Exceptions;

public class ExceededCurrentCapacityException : Exception
{
    public ExceededCurrentCapacityException(string message)
        : base(message)
    {
    }
}