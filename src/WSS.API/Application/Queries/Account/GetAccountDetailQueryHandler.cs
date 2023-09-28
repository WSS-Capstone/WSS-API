using WSS.API.Data.Repositories.Account;

namespace WSS.API.Application.Queries.Account;

public class GetAccountDetailQueryHandler : IRequestHandler<GetAccountDetailQuery, AccountResponse>
{
    private IMapper _mapper;
    private IAccountRepo _accountRepo;

    public GetAccountDetailQueryHandler(IMapper mapper, IAccountRepo accountRepo)
    {
        _mapper = mapper;
        _accountRepo = accountRepo;
    }

    public async Task<AccountResponse> Handle(GetAccountDetailQuery request, CancellationToken cancellationToken)
    {
        var query = await _accountRepo.GetAccountById(request.Id, new Expression<Func<Data.Models.Account, object>>[]
        {
            account => account.Customer,
            a => a.Partner,
            a => a.Owner,
        });
        
        var result = this._mapper.Map<AccountResponse>(query);

        return result;
    }
}