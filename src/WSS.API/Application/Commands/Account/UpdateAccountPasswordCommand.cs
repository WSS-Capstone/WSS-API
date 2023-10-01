using FirebaseAdmin.Auth;
using WSS.API.Data.Repositories.Account;

namespace WSS.API.Application.Commands.Account;

/// <inheritdoc />
public class UpdateAccountPasswordCommand : IRequest<AccountResponse>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class UpdatePasswordRequest{
    public string Password { get; set; }
}

public class UpdateAccountPasswordCommandHandler : IRequestHandler<UpdateAccountPasswordCommand, AccountResponse>
{
    private IMapper _mapper;
    private readonly IAccountRepo _accountRepo;
    private FirebaseAuth _firebaseAuth;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="accountRepo"></param>
    /// <param name="firebaseAuth"></param>
    public UpdateAccountPasswordCommandHandler(IAccountRepo accountRepo, FirebaseAuth firebaseAuth, IMapper mapper)
    {
        _firebaseAuth = firebaseAuth;
        _mapper = mapper;
        _accountRepo = accountRepo;
    }

    public async Task<AccountResponse> Handle(UpdateAccountPasswordCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepo.GetAccounts(a => a.Username == request.Email).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        
        if (account == null)
        {
            throw new Exception("Account not found");
        }

        var userInFb = await this._firebaseAuth.GetUserByEmailAsync(request.Email, cancellationToken);
        if (userInFb == null)
        {
            throw new Exception("Account not found");
        }

        userInFb = await this._firebaseAuth.UpdateUserAsync(new UserRecordArgs()
        {
            Uid = userInFb.Uid,
            Password = request.Password,
            DisplayName = userInFb.DisplayName,
            PhoneNumber = userInFb.PhoneNumber,
            Email = userInFb.Email,
            EmailVerified = userInFb.EmailVerified,
            PhotoUrl = userInFb.PhotoUrl,
            Disabled = userInFb.Disabled
        }, cancellationToken);
        
        
        var result = _mapper.Map<AccountResponse>(account);
        
        return result;
    }
}