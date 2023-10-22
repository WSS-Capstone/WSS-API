namespace WSS.API.Application.Models.ViewModels;

public class PaymentHistoryResponse
{
    public Guid Id { get; set; }
    public Guid? OrderId { get; set; }
    public string? PaymentType { get; set; }
    public Guid? RequestUserid { get; set; }
    public double? TotalAmount { get; set; }
    public DateTime? CreateDate { get; set; }
}

public enum PaymentHistoryStatus
{
    PENDING = 1,
    DEPOSITING = 2,
    DEPOSITED = 3,
    DONE = 4,
    CANCEL = 5,
}