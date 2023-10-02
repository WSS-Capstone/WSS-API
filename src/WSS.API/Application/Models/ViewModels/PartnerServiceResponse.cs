namespace WSS.API.Application.Models.ViewModels;

public class PartnerServiceResponse
{
    public Guid Id { get; set; }
    public Guid? PartnerId { get; set; }
    public Guid? ServiceId { get; set; }
    public int? Quantity { get; set; }
    public PartnerServiceStatus? Status { get; set; }
    public string? ImageUrl { get; set; }
    public double? Priority { get; set; }
}

public enum PartnerServiceStatus
{
    ACTIVE = 1,
    INACTIVE = 2,
    DELETED = 3,
}