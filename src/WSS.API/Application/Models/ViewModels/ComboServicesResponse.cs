namespace WSS.API.Application.Models.ViewModels;

public class ComboServicesResponse
{
    public Guid Id { get; set; }
    public virtual ServiceResponse Service { get; set; }
    public virtual ComboResponse Combo { get; set; }
}
