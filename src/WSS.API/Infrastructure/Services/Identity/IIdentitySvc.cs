namespace WSS.API.Infrastructure.Services.Identity;

/// <summary>
/// Handle identity request.
/// </summary>
public interface IIdentitySvc
{
    public string? GetUserRefId();
    
    public Task<Guid> GetUserId();

    public string? GetEmail();

    public bool IsActive();
}