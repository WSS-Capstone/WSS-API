using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using WSS.API.Data.Repositories.Account;
using WSS.API.Infrastructure.Config;
using WSS.API.Infrastructure.Services.Identity;
using WSS.API.Infrastructure.Utilities;
using Task = System.Threading.Tasks.Task;

namespace WSS.API.Infrastructure.Middleware;

public class UserFireFilter : IAsyncActionFilter
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAccountRepo _accountRepo;
    private readonly IIdentitySvc _identitySvc;
    private readonly FirebaseAuth _firebaseAuth;

    public UserFireFilter(IHttpContextAccessor httpContextAccessor, IAccountRepo accountRepo, IIdentitySvc identitySvc, FirebaseAuth firebaseAuth)
    {
        _httpContextAccessor = httpContextAccessor;
        _accountRepo = accountRepo;
        _identitySvc = identitySvc;
        _firebaseAuth = firebaseAuth;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var userId = _identitySvc.GetUserRefId();

        if (!context.ActionDescriptor.DisplayName.Contains("Login"))
        {
            await next();
            return;
        }
        
        if (userId != null)
        {
            var user = _accountRepo.GetAccounts(account => account.RefId == userId).FirstOrDefaultAsync();
            var userInFb = this._firebaseAuth.GetUserAsync(userId);
            await Task.WhenAll(new List<Task>(){ user, userInFb });

            if (user.Result != null)
            {
                await next();
                return;
            }
            
            // if (user?.Result.Status == (int)AccountStatus.InActive)
            // {
            //     context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
            //     await context.HttpContext.Response.WriteAsync("Account is not active");
            //     return;
            // }
            // if (!userInFb.Result.EmailVerified)
            // {
            //     context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
            //
            //     await context.HttpContext.Response.WriteAsync(string.Empty);
            //     return;
            // }
            
            var code = await _accountRepo.GetAccounts().OrderByDescending(x => x.Code).Select(x => x.Code)
                .FirstOrDefaultAsync();
            var newId = Guid.NewGuid();
            if (user.Result == null && userInFb.Result != null)
            {
                await this._accountRepo.CreateAccount(new Account()
                {
                    Id = newId,
                    Code = GenCode.NextId(code, "A"),
                    Username = userInFb.Result.Email,
                    RefId = userId,
                    RoleName = RoleName.CUSTOMER,
                    Status = (int?)AccountStatus.Active,
                    User = new User()
                    {
                        Id = newId,
                    }
                    // Status = userInFb.Result.EmailVerified ? 1 : 0,
                });
            } else if (user.Result != null && userInFb.Result != null)
            {
                // user.Result.Status = userInFb.Result.EmailVerified ? 1 : 0;
                // await this._accountRepo.UpdateAccount(user.Result);
            }
        }
        await next();
    }
}