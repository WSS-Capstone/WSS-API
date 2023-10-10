namespace WSS.API.Application.Models.ViewModels;

public class ComboResponse
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public double? DiscountValueCombo { get; set; }
    public double? TotalAmount { get; set; }
    public string? Description { get; set; }
    public ComboStatus? Status { get; set; }
    
    public List<ComboServicesResponse>? ComboServices { get; set; }
    public virtual ICollection<OrderResponse> Orders { get; set; }
}

public enum ComboStatus
{
    Active = 1,
    InActive = 0
}
