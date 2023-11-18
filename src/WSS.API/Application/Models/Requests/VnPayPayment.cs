using System.ComponentModel;

namespace WSS.API.Application.Models.Requests;

public class VnPayPayment
{
    public Guid OrderReferenceId { get; set; } = default!;
    public Guid CustomerId { get; set; } = default!;
    public double? Amount { get; set; }
    public OrderType OrderType { get; set; }
    public List<IFormFile> Image { get; set; }
}

public class VNPayRequest
{
    public Guid OrderReferenceId { get; set; } = default!;
    public OrderType OrderType { get; set; }
}

public class VNPayRequestPartner
{
    public Guid OrderReferenceId { get; set; } = default!;
    public OrderType OrderType { get; set; }
    public List<IFormFile>? Image { get; set; }
}

public enum OrderType
{
    [Description("Thanh toán")] 
    Payment,
    
    [Description("Cọc")] 
    Deposit
}