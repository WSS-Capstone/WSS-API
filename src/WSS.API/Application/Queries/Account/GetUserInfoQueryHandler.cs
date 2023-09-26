using MediatR;
using Microsoft.EntityFrameworkCore;
using WSS.API.Data.Repositories.Account;
using WSS.API.Infrastructure.Services.Identity;

namespace WSS.API.Application.Queries.Account;

public class GetUserInfoQueryHandler : IRequestHandler<GetUserInfoQuery, Data.Models.Account?>
{
    private IAccountRepo _accountRepo;
    private IIdentitySvc _identitySvc;

    public GetUserInfoQueryHandler(IAccountRepo accountRepo, IIdentitySvc identitySvc)
    {
        _accountRepo = accountRepo;
        _identitySvc = identitySvc;
    }

    public async Task<Data.Models.Account?> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        var userId = _identitySvc.GetUserId();
        return await this._accountRepo.GetAccounts(a => a.RefId == userId).FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }
}