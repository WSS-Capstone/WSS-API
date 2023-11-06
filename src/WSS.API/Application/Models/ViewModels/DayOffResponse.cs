using WSS.API.Application.Queries.DayOff;

namespace WSS.API.Application.Models.ViewModels;

public class DayOffResponse
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public Guid? PartnerId { get; set; }
    public DateTime? Day { get; set; }
    public string? Reason { get; set; }
    public DayOffStatus? Status { get; set; }
    public virtual UserResponse? Partner { get; set; }
    public virtual ServiceResponse? Service { get; set; }
}