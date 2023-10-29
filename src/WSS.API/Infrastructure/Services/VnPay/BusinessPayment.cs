namespace WSS.API.Infrastructure.Services.VnPay;

public class BusinessPayment
{
    public Guid OrderId { get; set; }
    public long Amount { get; set; }
}