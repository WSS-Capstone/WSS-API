using MediatR;
using Microsoft.EntityFrameworkCore;
using WSS.API.Data.Repositories.Account;
using WSS.API.Infrastructure.Services.Identity;

namespace WSS.API.Application.Queries.Account;

public class GetUserInfoQueryHandler : IRequestHandler<GetUserInfoQuery, AccountResponse>
{
    private IMapper _mapper;
    private IAccountRepo _accountRepo;
    private IIdentitySvc _identitySvc;

    public GetUserInfoQueryHandler(IAccountRepo accountRepo, IIdentitySvc identitySvc, IMapper mapper)
    {
        _accountRepo = accountRepo;
        _identitySvc = identitySvc;
        _mapper = mapper;
    }

    public async Task<AccountResponse> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        var userId = _identitySvc.GetUserId();
        var result = await this._accountRepo.GetAccounts(a => a.RefId == userId, new Expression<Func<Data.Models.Account, object>>[]
        {
            account => account.Customer,
            a => a.Partner,
            a => a.Owner,
        }).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        return this._mapper.Map<AccountResponse>(result);
    }
}