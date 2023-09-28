namespace WSS.API.Application.Models.ViewModels;

public class AccountResponse
{
    public Guid Id { get; set; }
    public string? Username { get; set; }
    public AccountStatus? Status { get; set; }
    public string? RefId { get; set; }
    public string? RoleName { get; set; }
    public virtual CustomerResponse? Customer { get; set; }
    public virtual OwnerResponse? Owner { get; set; }
    public virtual PartnerResponse? Partner { get; set; }
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