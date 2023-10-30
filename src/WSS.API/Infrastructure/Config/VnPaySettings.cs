using System.ComponentModel.DataAnnotations;

namespace WSS.API.Infrastructure.Config;

public class VnPaySettings
{
    public static readonly string ConfigSection = "VnPay";
    
    [Required]
    public string Version { get; set; } = default!;

    [Required]
    public string TmnCode { get; set; } = default!;

    [Required]
    public string HashSecret { get; set; } = default!;

    [Required]
    public string PaymentEndpoint { get; set; } = default!;

    [Required]
    public string CallbackUrl { get; set; } = default!;
}