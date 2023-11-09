using System.ComponentModel;

namespace WSS.API.Application.Models.Requests;

public class VnPayPayment
{
    public Guid OrderReferenceId { get; set; } = default!;
    public Guid CustomerId { get; set; } = default!;
    public long Amount { get; set; }
    public OrderType OrderType { get; set; }
}
public enum OrderType
{
    [Description("Thanh toán")] 
    Payment,
    
    [Description("Cọc")] 
    Deposit
}