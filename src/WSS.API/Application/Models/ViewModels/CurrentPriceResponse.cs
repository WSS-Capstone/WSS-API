namespace WSS.API.Application.Models.ViewModels;

public class CurrentPriceResponse
{
    public Guid Id { get; set; }
    public DateTime? DateOfApply { get; set; }
    public ServiceResponse? Service { get; set; }
    public double? Price { get; set; }

}