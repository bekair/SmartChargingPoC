﻿namespace SmartChargingPoC.DataAccess.Exceptions;

public class DataNotFoundException : Exception
{
    public DataNotFoundException(string message)
        : base(message)
    {
    }
}