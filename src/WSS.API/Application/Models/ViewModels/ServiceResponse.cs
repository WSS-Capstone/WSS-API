namespace WSS.API.Application.Models.ViewModels;

public class ServiceResponse
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public int? Quantity { get; set; }
    public string? CoverUrl { get; set; }
    public Guid? Categoryid { get; set; }
    public Guid? OwnerId { get; set; }
    public string? Description { get; set; }
    public ServiceStatus? Status { get; set; }
}
public enum ServiceStatus
{
    ACTIVE = 1,
    INACTIVE = 2,
    DELETED = 3,
}
