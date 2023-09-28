namespace WSS.API.Application.Models.ViewModels;

public class OrderResponse
{
    public Guid Id { get; set; }
    public Guid? CustomerId { get; set; }
    public Guid? OwnerId { get; set; }
    public Guid? WeddingInformationId { get; set; }
    public string? Fullname { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public Guid? VoucherId { get; set; }
    public Guid? ComboId { get; set; }
    public double? TotalAmount { get; set; }
    public double? TotalAmountRequest { get; set; }
    public string? Description { get; set; }
    public DateTime? CreateDate { get; set; }
    public OrderStatus? Status { get; set; }
}

public enum OrderStatus
{
    ACTIVE = 1,
    INACTIVE = 2,
    DELETED = 3,
}