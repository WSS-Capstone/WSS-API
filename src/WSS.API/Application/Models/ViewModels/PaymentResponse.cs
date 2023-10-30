using WSS.API.Application.Models.Requests;

namespace WSS.API.Application.Models.ViewModels;

public class PaymentResponse
{
    public string? LinkPay { get; set; }
    public PaymentStatus Status { get; set; }
    public OrderType OrderType { get; set; }
}
public enum PaymentStatus
{
    Pending = 1,
    Success = 2,
    Failed = 3,
    Cancelled = 4
}