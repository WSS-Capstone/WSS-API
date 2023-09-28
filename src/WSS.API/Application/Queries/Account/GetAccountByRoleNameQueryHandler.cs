using WSS.API.Data.Repositories.Account;

namespace WSS.API.Application.Queries.Account;

public class GetAccountByRoleNameQueryHandler :  IRequestHandler<GetAccountsByRoleNameQuery, PagingResponseQuery<AccountResponse, AccountSortCriteria>>
{
    private IMapper _mapper;
    private IAccountRepo _accountRepo;

    public GetAccountByRoleNameQueryHandler(IMapper mapper, IAccountRepo accountRepo)
    {
        _mapper = mapper;
        _accountRepo = accountRepo;
    }


    /// <inheritdoc />
    public async Task<PagingResponseQuery<AccountResponse, AccountSortCriteria>> Handle(GetAccountsByRoleNameQuery request, CancellationToken cancellationToken)
    {
        var query = _accountRepo.GetAccounts(a => a.RoleName == request.RoleName.ToString());
        var total = await query.CountAsync(cancellationToken: cancellationToken);
        
        query = query.GetWithSorting(request.SortKey.ToString(), request.SortOrder);
        
        query = query.GetWithPaging(request.Page, request.PageSize);

        var result = this._mapper.ProjectTo<AccountResponse>(query);

        return new PagingResponseQuery<AccountResponse, AccountSortCriteria>(request, result, total);
    }
}