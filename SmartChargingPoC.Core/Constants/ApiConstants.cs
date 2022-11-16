namespace SmartChargingPoC.Core.Constants;

public static class ApiConstants
{
    public static class ErrorMessage
    {
        public const string InvalidPropertyType = "The Property must be {0}.";
        public const string Required = "{0} is required.";
        public const string GreaterThan = "{0} must be greater than {1}.";
        public const string Range = "{0} must be between {1} and {2}.";
        public const string NotExistedGroupChargeStationOperation = $"You need a {PropertyName.Group} to add the {PropertyName.ChargeStation}.";
        public const string NotExistedChargeStationInConnectorOperation = $"You need a {PropertyName.ChargeStation} to add the {PropertyName.Connector}.";
        public const string ExceededCurrentCapacity = $"You have exceeded the Current Capacity in the {PropertyName.Group}.";
        public const string DataNotFound = "The {0} could not be found with the requested id.";
        public const string NoConnectorSlotInChargeStation = $"The {PropertyName.ChargeStation} has no slot for a new {PropertyName.Connector}";
        public const string NotExistedLastConnectorUniqueNumber = $"The last connector unique number for the {PropertyName.ChargeStationId}: {{0}} could not be found";
        public const string NotExistedGroupIdByChargeStationId = $"The {PropertyName.GroupId} could not be found with the requested {PropertyName.ChargeStationId}.";
        public const string NotExistedGroupIdByConnectorId = $"The {PropertyName.GroupId} could not be found with the requested {PropertyName.ConnectorId}.";
    }

    public static class PropertyName
    {
        public const string ChargeStation = "Charge Station";
        public const string Group = "Group";
        public const string GroupId = "Group Id";
        public const string Connector = "Connector";
        public const string ConnectorId = "Connector Id";
        public const string ConnectorMaxCurrentInAmps = "Max Current(in Amps) of Connector";
        public const string MaxCurrentInAmps = "Max Current(in Amps) of Connector";
        public const string CapacityInAmps = "Capacity In Amps";
        public const string Name = "Name";
        public const string ChargeStationId = "Charge Station Id";
        public const string ChargeStationUniqueNumber = "Charge Station Unique Number";
    }

    public static class HttpMethod
    {
        public const string Options = "OPTIONS";
        public const string Get = "GET";
        public const string Post = "POST";
        public const string Put = "PUT";
        public const string Delete = "DELETE";
        public const string Patch = "PATCH";
    }
    
    public static class General
    {
        public const string ContentType = "application/json";
        public const string SmartChargingDbName = "SmartChargingDb";
        public const string RequestedParameter = "Requested Model";
        public const double MinCapacityInAmpsValue = 0;
    }
}