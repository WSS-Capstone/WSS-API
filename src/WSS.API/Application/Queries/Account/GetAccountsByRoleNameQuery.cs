using WSS.API.Data.Repositories.Account;
using WSS.API.Infrastructure.Config;

namespace WSS.API.Application.Queries.Account;

public class GetAccountsByRoleNameQuery : PagingParam<AccountSortCriteria>, IRequest<PagingResponseQuery<AccountResponse, AccountSortCriteria>>
{
    public GetAccountsByRoleNameQuery(RoleEnum roleName)
    {
        RoleName = roleName;
    }

    public RoleEnum RoleName { get; set; }
}

public enum AccountSortCriteria
{
    Id,
    Username,
    Status,
    CreateDate
}

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