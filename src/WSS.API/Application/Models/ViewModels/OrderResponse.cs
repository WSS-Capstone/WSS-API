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
    public virtual ComboResponse? Combo { get; set; }
    public virtual UserResponse? Customer { get; set; }
    public virtual VoucherResponse? Voucher { get; set; }
    public virtual WeddingInformationResponse? WeddingInformation { get; set; }
    public virtual List<OrderDetailResponse> OrderDetails { get; set; }
}

public enum StatusOrder
{
    Pending = 1,
    Confirm = 2,
    Doing = 3,
    Done = 4,
    Cancel = 5,
}
public enum StatusPayment
{
    Pending = 1,
    Confirm = 2, // Cho Dat coc
    Doing = 3, // Da dat coc/
    Done = 4,
    Cancel = 5,
}