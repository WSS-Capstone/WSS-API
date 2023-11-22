namespace WSS.API.Application.Models.ViewModels;

public class PartnerPaymentHistoryResponse
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public Guid? PartnerId { get; set; }
    public Guid? OrderId { get; set; }
    public PartnerPaymentHistoryStatus? Status { get; set; }
    public double? Total { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime? CreateDate { get; set; }
    
    public virtual OrderResponse? Order { get; set; }
    public virtual UserResponse? Partner { get; set; }
}

public enum PartnerPaymentHistoryStatus
{
    ACTIVE = 1,
    INACTIVE = 2,
    DELETED = 3,
}