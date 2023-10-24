namespace WSS.API.Application.Models.ViewModels;

public class OrderResponse
{
    public string? Code { get; set; }
    public Guid Id { get; set; }
    public Guid? CustomerId { get; set; }
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
    public Guid? CreateBy { get; set; }
    public StatusPayment? StatusPayment { get; set; }
    public StatusOrder? StatusOrder { get; set; }
}

public enum StatusOrder
{
    PENDING = 1,
    CONFIRM = 2,
    DOING = 3,
    DONE = 4,
    CANCEL = 5,
}
public enum StatusPayment
{
    PENDING = 1,
    CONFIRM = 2,
    DOING = 3,
    DONE = 4,
    CANCEL = 5,
}