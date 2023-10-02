namespace WSS.API.Application.Models.ViewModels;

public class ServiceImageResponse
{
    public Guid Id { get; set; }
    public string? ImageUrl { get; set; }
    public Guid? ServiceId { get; set; }
}