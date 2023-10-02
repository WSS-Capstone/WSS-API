namespace WSS.API.Application.Models.ViewModels;

public class RoleResponse
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? IsUser { get; set; }
}