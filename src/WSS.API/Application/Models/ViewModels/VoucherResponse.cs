namespace WSS.API.Application.Models.ViewModels;

public class VoucherResponse
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? ImageUrl { get; set; }
    public double? DiscountValueVoucher { get; set; }
    public double? MinAmount { get; set; }
    public DateTime? CreateDate { get; set; }
    public DateTime? EndTime { get; set; }
    public DateTime? StartTime { get; set; }
    public Guid? CreateBy { get; set; }
}

public enum VouncherStatus
{
    ACTIVE = 1,
    INACTIVE = 2,
    DELETED = 3,
}