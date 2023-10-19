using FirebaseAdmin.Auth;
using MediatR;
using WSS.API.Data.Repositories.Account;
using WSS.API.Data.Repositories.User;
using WSS.API.Infrastructure.Config;
using Task = System.Threading.Tasks.Task;

namespace WSS.API.Application.Commands.Account;

public class CreateAccountForCustomerCommand : IRequest<Data.Models.Account?>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Fullname { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public int Gender { get; set; }
}

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountForCustomerCommand, Data.Models.Account?>
{
    private IAccountRepo _accountRepo;
    private FirebaseAuth _firebaseAuth;
    private IUserRepo _userRepo;

    public CreateAccountCommandHandler(IAccountRepo accountRepo, FirebaseAuth firebaseAuth, IUserRepo userRepo)
    {
        _accountRepo = accountRepo;
        _firebaseAuth = firebaseAuth;
        _userRepo = userRepo;
    }

    public async Task<Data.Models.Account?> Handle(CreateAccountForCustomerCommand request,
        CancellationToken cancellationToken)
    {
        var user = await this._firebaseAuth.GetUserByEmailAsync(request.Email, cancellationToken);
        if (user != null)
        {
            return null;
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
                Address = request.Address,
                Gender = request.Gender,
            }
        });

        await Task.WhenAll(new List<Task> { userFb, userDb });

        return await this._accountRepo.GetAccountById(id);
    }
}