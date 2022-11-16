namespace SmartChargingPoC.Business.Dtos.Responses.Groups;

public class CreateGroupResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double CapacityInAmps { get; set; }
}