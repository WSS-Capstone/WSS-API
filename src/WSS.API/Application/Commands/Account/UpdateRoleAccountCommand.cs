using FirebaseAdmin.Auth;
using WSS.API.Data.Repositories.Account;
using WSS.API.Data.Repositories.Customer;
using WSS.API.Data.Repositories.Owner;
using WSS.API.Data.Repositories.Partner;
using WSS.API.Data.Repositories.staff;
using WSS.API.Infrastructure.Config;

namespace WSS.API.Application.Commands.Account;

public class UpdateRoleAccountCommand : IRequest<AccountResponse>
{
    public string Email { get; set; }
    public RoleEnum RoleName { get; set; }
}

public class UpdateRoleAccountRequest
{
    public RoleEnum RoleName { get; set; }
}

public class UpdateRoleAccountCommandHandler : IRequestHandler<UpdateRoleAccountCommand, AccountResponse>
{
    private IMapper _mapper;
    private readonly IAccountRepo _accountRepo;
    private readonly IPartnerRepo _partnerRepo;
    private readonly IStaffRepo _staffRepo;
    private readonly ICustomerRepo _customerRepo;
    private readonly IOwnerRepo _ownerRepo;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="accountRepo"></param>
    /// <param name="mapper"></param>
    /// <param name="partnerRepo"></param>
    /// <param name="staffRepo"></param>
    /// <param name="customerRepo"></param>
    /// <param name="ownerRepo"></param>
    public UpdateRoleAccountCommandHandler(IAccountRepo accountRepo, IMapper mapper, IPartnerRepo partnerRepo, IStaffRepo staffRepo, ICustomerRepo customerRepo, IOwnerRepo ownerRepo)
    {
        _mapper = mapper;
        _partnerRepo = partnerRepo;
        _staffRepo = staffRepo;
        _customerRepo = customerRepo;
        _ownerRepo = ownerRepo;
        _accountRepo = accountRepo;
    }

    public async Task<AccountResponse> Handle(UpdateRoleAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepo.GetAccounts(a => a.Username == request.Email, new Expression<Func<Data.Models.Account, object>>[]
        {
            a => a.Owner,
            a => a.staff,
            a => a.Partner,
            a => a.Customer,
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