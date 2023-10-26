namespace WSS.API.Application.Models.ViewModels;

public class DayOffResponse
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public Guid? PartnerId { get; set; }
    public DateTime? Day { get; set; }
    public string? Reason { get; set; }
}