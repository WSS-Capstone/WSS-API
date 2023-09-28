namespace WSS.API.Application.Models.ViewModels;

public class PartnerResponse
{
    public Guid Id { get; set; }
    public string? Fullname { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? ImageUrl { get; set; }
    public Gender? Gender { get; set; }
    public Guid? RoleId { get; set; }
}