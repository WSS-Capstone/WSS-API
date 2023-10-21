namespace WSS.API.Application.Models.ViewModels;

public class AccountResponse
{
    public Guid Id { get; set; }
    public string? Username { get; set; }
    public AccountStatus? Status { get; set; }
    public string? RoleName { get; set; }
    public virtual UserResponse? User { get; set; }
}

public enum AccountStatus
{
    Active = 1,
    InActive = 0
}

public enum Gender
{
    Male = 1,
    Female = 0,
    Other = 2
}