namespace WSS.API.Infrastructure.Services.VnPay;

public class BusinessPayment
{
    public string Ip { get; set; }
    public Guid CompanyId { get; set; }
    public long Amount { get; set; }
}