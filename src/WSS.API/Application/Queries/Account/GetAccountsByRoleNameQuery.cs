using Microsoft.EntityFrameworkCore.Query.Internal;
using WSS.API.Data.Repositories.Account;
using WSS.API.Infrastructure.Config;

namespace WSS.API.Application.Queries.Account;

public class GetAccountsByRoleNameQuery : PagingParam<AccountSortCriteria>,
    IRequest<PagingResponseQuery<AccountResponse, AccountSortCriteria>>
{
    public List<RoleEnum> RoleNames { get; set; } = new List<RoleEnum>()
        { RoleEnum.Customer, RoleEnum.Owner, RoleEnum.Partner, RoleEnum.Staff };
}

public enum AccountSortCriteria
{
    Id,
    Username,
    Status,
    CreateDate
}

public class GetAccountByRoleNameQueryHandler : IRequestHandler<GetAccountsByRoleNameQuery,
    PagingResponseQuery<AccountResponse, AccountSortCriteria>>
{
    private IMapper _mapper;
    private IAccountRepo _accountRepo;

    public GetAccountByRoleNameQueryHandler(IMapper mapper, IAccountRepo accountRepo)
    {
        _mapper = mapper;
        _accountRepo = accountRepo;
    }


    /// <inheritdoc />
    public async Task<PagingResponseQuery<AccountResponse, AccountSortCriteria>> Handle(
        GetAccountsByRoleNameQuery request, CancellationToken cancellationToken)
    {
        var roleNames = request.RoleNames.Select(role => role.ToString()).ToList();
        var query = _accountRepo.GetAccounts(a => roleNames.Contains(a.RoleName));
        var total = await query.CountAsync(cancellationToken: cancellationToken);

        query = query.GetWithSorting(request.SortKey.ToString(), request.SortOrder);

        query = query.GetWithPaging(request.Page, request.PageSize);

        var result = this._mapper.ProjectTo<AccountResponse>(query);

        return new PagingResponseQuery<AccountResponse, AccountSortCriteria>(request, result, total);
    }
}