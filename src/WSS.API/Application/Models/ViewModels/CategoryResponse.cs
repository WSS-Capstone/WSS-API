namespace WSS.API.Application.Models.ViewModels;

public class CategoryResponse
{
    public Guid Id { get; set; }
    public Guid? CategoryId { get; set; }
    public string? Name { get; set; }
    public string? ImageUrl { get; set; }
    public string? Description { get; set; }
    public CategoryStatus Status { get; set; }
    public bool? IsOrderLimit { get; set; }
    public virtual CommissionResponse Commission { get; set; }
    public virtual ICollection<ServiceResponse> Services { get; set; }
}

public enum CategoryStatus
{
    Active = 1,
    InActive = 0
}
