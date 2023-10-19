using WSS.API.Data.Repositories.Account;
using WSS.API.Infrastructure.Config;

namespace WSS.API.Application.Commands.Account;

public class UpdateRoleAccountCommand : IRequest<AccountResponse>
{
    public string Email { get; set; }
    public RoleEnum RoleName { get; set; }
}

public class UpdateRoleAccountCommandHandler : IRequestHandler<UpdateRoleAccountCommand, AccountResponse>
{
    private IMapper _mapper;
    private readonly IAccountRepo _accountRepo;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="accountRepo"></param>
    /// <param name="mapper"></param>
    public UpdateRoleAccountCommandHandler(IMapper mapper, IAccountRepo accountRepo)
    {
        _mapper = mapper;
        _accountRepo = accountRepo;
    }

    public async Task<AccountResponse> Handle(UpdateRoleAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepo.GetAccounts(a => a.Username == request.Email, new Expression<Func<Data.Models.Account, object>>[]
        {
            a => a.User
        }).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        
        if (account == null)
        {
            throw new Exception("Account not found");
        }

        var info = RoleName.CUSTOMER;
        // TODO: Implement update role account
        
        
        return this._mapper.Map<AccountResponse>(account);
    }
}