namespace WSS.API.Application.Models.ViewModels;

public class CartResponse
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public Guid? ServiceId { get; set; }
}