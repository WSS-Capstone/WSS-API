using FirebaseAdmin.Auth;
using WSS.API.Data.Repositories.Account;
using WSS.API.Infrastructure.Config;

namespace WSS.API.Application.Commands.Account;

/// <inheritdoc />
public class UpdateAccountProfileCommand : IRequest<AccountResponse>
{
    public UpdateAccountProfileCommand(string email, UpdateMyAccountProfileCommand? command)
    {
        Email = email;
        Password = command?.Password;
        Fullname = command?.Fullname;
        DateOfBirth = command?.DateOfBirth;
        Phone = command?.Phone;
        Address = command?.Address;
        Gender = (Gender)command?.Gender;
        CategoryId = command?.CategoryId;
        RoleName = command?.RoleName;
        Status = command?.Status;
        ImageUrl = command?.ImageUrl;
        Reason = command?.Reason;
    }

    public string Email { get; set; }
    public string? Password { get; set; }
    public string? Fullname { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public Gender Gender { get; set; }
    public string? ImageUrl { get; set; }
    public Guid? CategoryId { get; set; }
    public string? Reason { get; set; }
    public RoleEnum? RoleName { get; set; }
    public AccountStatus? Status { get; set; }
}

public class UpdateMyAccountProfileCommand
{
    public string? Password { get; set; }
    public string? Fullname { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public Gender Gender { get; set; }
    public string? ImageUrl { get; set; }
    public Guid? CategoryId { get; set; }
    public RoleEnum? RoleName { get; set; }
    public AccountStatus? Status { get; set; }
    public string? Reason { get; set; }
}


public class UpdateAccountPasswordCommandHandler : IRequestHandler<UpdateAccountProfileCommand, AccountResponse>
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

    public async Task<AccountResponse> Handle(UpdateAccountProfileCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepo.GetAccounts(a => a.Username == request.Email, new Expression<Func<Data.Models.Account, object>>[]
        {
            u => u.User
        }).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        
        if (account == null)
        {
            throw new Exception("Account not found");
        }

        var userInFb = await this._firebaseAuth.GetUserByEmailAsync(request.Email, cancellationToken);
        if (userInFb == null)
        {
            throw new Exception("Account not found");
        }

        account.User.Fullname = string.IsNullOrEmpty(request.Fullname) ? account.User?.Fullname : request.Fullname;
        account.User.Address = string.IsNullOrEmpty(request.Address) ? account.User?.Address : request.Address;
        account.User.Phone = string.IsNullOrEmpty(request.Phone) ? account.User?.Phone : request.Phone;
        account.User.CategoryId = request.CategoryId ?? account.User?.CategoryId;
        account.User.Gender = (int?)request.Gender ?? account.User?.Gender;
        account.User.ImageUrl = string.IsNullOrEmpty(request.ImageUrl) ? account.User?.ImageUrl : request.ImageUrl;
        account.User.DateOfBirth = request.DateOfBirth ?? account?.User?.DateOfBirth;
        account.RoleName = request.RoleName == null ? account.RoleName : request.RoleName.ToString();
        account.Status = request.Status == null ? account.Status : (int?)request.Status;
        account.Reason = string.IsNullOrEmpty(request.Reason) ? account.Reason : request.Reason;
        
        userInFb = await this._firebaseAuth.UpdateUserAsync(new UserRecordArgs()
        {
            Uid = userInFb.Uid,
            Password = string.IsNullOrEmpty(request.Password) ? null : request.Password,
            DisplayName = userInFb.DisplayName,
            PhoneNumber = userInFb.PhoneNumber,
            Email = userInFb.Email,
            EmailVerified = userInFb.EmailVerified,
            PhotoUrl = userInFb.PhotoUrl,
            Disabled = userInFb.Disabled
        }, cancellationToken);

        await this._accountRepo.UpdateAccount(account);
        
        
        var result = _mapper.Map<AccountResponse>(account);
        
        return result;
    }
}