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
    Status
}