using FirebaseAdmin.Auth;
using WSS.API.Data.Repositories.Account;
using WSS.API.Data.Repositories.User;
using WSS.API.Infrastructure.Config;

namespace WSS.API.Application.Commands.Account;

public class CreateAccountForAdminCommand : IRequest<AccountResponse>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Fullname { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public Gender Gender { get; set; }
    public string? ImageUrl { get; set; }
    public Guid? CategoryId { get; set; }
    public RoleEnum RoleName { get; set; }
}

public class CreateAccountForAdminHandler : IRequestHandler<CreateAccountForAdminCommand, AccountResponse>
{
    private IAccountRepo _accountRepo;
    private FirebaseAuth _firebaseAuth;
    private IUserRepo _userRepo;
    private IMapper _mapper;

    public CreateAccountForAdminHandler(IAccountRepo accountRepo, FirebaseAuth firebaseAuth, IUserRepo userRepo, IMapper mapper)
    {
        _accountRepo = accountRepo;
        _firebaseAuth = firebaseAuth;
        _userRepo = userRepo;
        _mapper = mapper;
    }

    public async Task<AccountResponse> Handle(CreateAccountForAdminCommand request,
        CancellationToken cancellationToken)
    {
        // var user = await this._firebaseAuth.GetUserByEmailAsync(request.Email, cancellationToken);
        // if (user != null)
        // {
        //     throw new Exception("Email already exists!");
        // }
        var user = await _accountRepo.GetAccountByMail(request.Email);
        if (user != null)
        {
            throw new Exception("Email already exists!");
        }

        var id = Guid.NewGuid();
        var uid = id.ToString();
        await this._firebaseAuth.CreateUserAsync(new UserRecordArgs()
        {
            Uid = uid,
            Email = request.Email,
            Password = request.Password,
            Disabled = false,
            DisplayName = request.Fullname,
            PhoneNumber = request.Phone
        }, cancellationToken);

        await _accountRepo.CreateAccount(new Data.Models.Account()
        {
            Id = id,
            RefId = uid,
            RoleName = request.RoleName.ToString(),
            Status = (int?)AccountStatus.Active,
            Username = request.Email,
            CreateDate = DateTime.Now
        });
        await this._userRepo.CreateUser(new Data.Models.User()
        {
            Id = id,
            Fullname = request.Fullname,
            DateOfBirth = request.DateOfBirth,
            Phone = request.Phone,
            Address = request.Address,
            Gender = (int?)request.Gender,
            ImageUrl = request.ImageUrl,
            CategoryId = request.CategoryId,
            CreateDate = DateTime.Now
        });

        var response = await _accountRepo.GetAccountById(id);
        var result = this._mapper.Map<AccountResponse>(response);
        return result;
    }
}