namespace WSS.API.Application.Models.ViewModels;

public class CommissionResponse
{
    public Guid Id { get; set; }
    public PartnerResponse Partner { get; set; }
    public DateTime? DateOfApply { get; set; }
    public double? CommisionValue { get; set; }
}