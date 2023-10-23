namespace WSS.API.Application.Models.ViewModels;

public class ServiceResponse
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? CoverUrl { get; set; }
    public CategoryResponse? Category { get; set; }
    public CurrentPriceResponse? CurrentPrices { get; set; }
    public virtual ICollection<ServiceImageResponse> ServiceImages { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? OwnerId { get; set; }
    public string? Description { get; set; }
    public ServiceStatus? Status { get; set; }
}
public enum ServiceStatus
{
    Active = 1,
    InActive = 2,
    Deleted = 3,
}
