namespace WSS.API.Application.Models.ViewModels;

public class ServiceResponse
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? CoverUrl { get; set; }
    public int? Quantity { get; set; }
    public CategoryResponse? Category { get; set; }
    public CurrentPriceResponse? CurrentPrices { get; set; }
    public virtual ICollection<ServiceImageResponse> ServiceImages { get; set; }
    public Guid? CategoryId { get; set; }
    public string? Unit { get; set; }
    public string? Description { get; set; }
    public ServiceStatus? Status { get; set; }
    public int? Used { get; set; }
    public double? Rating { get; set; }
    public string? Reason { get; set; }
}

public enum ServiceStatus
{
    Active = 1,
    InActive = 2,
    Deleted = 3,
    Pending = 4,
    Reject = 5
}