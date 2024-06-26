﻿using System.Security.Claims;
using WSS.API.Data.Repositories.Account;

namespace WSS.API.Infrastructure.Services.Identity;

/// <inheritdoc/>
public class IdentitySvc : IIdentitySvc
{
    private const string UserIdClaim = "user_id";
    private const string EmailClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
    private const string VerifyClaim = "email_verified";
    private const string AuthRefIdClaim = ClaimTypes.NameIdentifier;

    private readonly IHttpContextAccessor context;
    private readonly IAccountRepo userRepo;

    /// <summary>
    /// Initializes a new instance of the <see cref="IdentitySvc"/> class.
    /// </summary>
    /// <param name="context">inject an instance of <see cref="IHttpContextAccessor"/>.</param>
    /// <param name="userRepo">Inject an instance of <see cref="IAccountRepo"/>.</param>
    public IdentitySvc(IHttpContextAccessor context, IAccountRepo userRepo)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        this.userRepo = userRepo;
    }

    public string? GetUserRefId()
    {
        return this.context.HttpContext.User.FindFirst(UserIdClaim)?.Value;
    }

    public async Task<Guid> GetUserId()
    {
        var user = await this.userRepo.GetAccounts(a => a.RefId == this.GetUserRefId()).FirstOrDefaultAsync();
        
        if (user == null)
        {
            throw new Exception("User not found");
        }
        
        return user.Id;
    }

    public string? GetEmail()
    {
        return this.context.HttpContext.User.FindFirst(EmailClaim)?.Value;
    }

    public bool IsActive()
    {
        var value = this.context.HttpContext.User.FindFirst(VerifyClaim)?.Value;
        if (value == null)
        {
            return false;
        }
        bool.TryParse(value, out var result);
        return result;
    }
}