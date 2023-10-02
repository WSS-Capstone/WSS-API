namespace WSS.API.Application.Models.ViewModels;

public class ComboServicesResponse
{
    public Guid Id { get; set; }
    public ServiceResponse Service { get; set; }
    public ComboService Combo { get; set; }
}
