using System.Security.Principal;
using FirebaseAdmin.Auth;
using WSS.API.Data.Repositories.Account;
using WSS.API.Data.Repositories.User;
using WSS.API.Infrastructure.Config;

namespace WSS.API.Application.Commands.Account;

public class CreateAccountForCustomerCommand : IRequest<AccountResponse>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Fullname { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string? Avatar { get; set; }
    public DateTime? Dob { get; set; }
    public Gender Gender { get; set; }
}

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountForCustomerCommand, AccountResponse>
{
    private IAccountRepo _accountRepo;
    private FirebaseAuth _firebaseAuth;
    private IUserRepo _userRepo;
    private IMapper _mapper;
    

    public CreateAccountCommandHandler(IAccountRepo accountRepo, FirebaseAuth firebaseAuth, IUserRepo userRepo, IMapper mapper)
    {
        _accountRepo = accountRepo;
        _firebaseAuth = firebaseAuth;
        _userRepo = userRepo;
        _mapper = mapper;
    }

    public async Task<AccountResponse> Handle(CreateAccountForCustomerCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var user = await this._firebaseAuth.GetUserByEmailAsync(request.Email, cancellationToken);
            if (user != null)
            {
                throw new Exception("Email đã tồn tại");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        var id = Guid.NewGuid();
        var uid = id.ToString();
        var userFb = this._firebaseAuth.CreateUserAsync(new UserRecordArgs()
        {
            Uid = uid,
            Email = request.Email,
            Password = request.Password,
            Disabled = false,
            DisplayName = request.Fullname,
            PhoneNumber = request.Phone,
        }, cancellationToken);
        var userDb = this._accountRepo.CreateAccount(new Data.Models.Account()
        {
            Id = id,
            RefId = uid,
            RoleName = RoleName.CUSTOMER,
            Status = 0,
            Username = request.Email,
            User = new Data.Models.User()
            {
                Fullname = request.Fullname,
                Phone = request.Phone,
                ImageUrl = request.Avatar,
                DateOfBirth = request.Dob,
                Address = request.Address,
                Gender = (int?)request.Gender,
            }
        });

        await System.Threading.Tasks.Task.WhenAll(new List<System.Threading.Tasks.Task> { userFb, userDb });
        var account = await this._accountRepo.GetAccountById(id);
        var result = this._mapper.Map<AccountResponse>(account);
        return result;
    }
}