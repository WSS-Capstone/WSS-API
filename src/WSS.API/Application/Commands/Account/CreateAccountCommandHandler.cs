using FirebaseAdmin.Auth;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WSS.API.Data.Repositories.Account;
using WSS.API.Data.Repositories.Customer;
using WSS.API.Infrastructure.Config;
using Task = System.Threading.Tasks.Task;

namespace WSS.API.Application.Commands.Account;

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, Data.Models.Customer?>
{
    private IAccountRepo _accountRepo;
    private FirebaseAuth _firebaseAuth;
    private ICustomerRepo _customerRepo;

    public CreateAccountCommandHandler(IAccountRepo accountRepo, FirebaseAuth firebaseAuth, ICustomerRepo customerRepo)
    {
        _accountRepo = accountRepo;
        _firebaseAuth = firebaseAuth;
        _customerRepo = customerRepo;
    }

    public async Task<Customer?> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
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
            RoleName = request.RoleName.ToString(),
            Status = 0,
            Username = request.Email,
            Customer = request.RoleName.ToString() == RoleName.CUSTOMER
                ? new Customer()
                {
                    Fullname = request.Fullname,
                    Phone = request.Phone,
                    Address = request.Address,
                    Gender = request.Gender,
                }
                : null,
            staff = request.RoleName.ToString() == RoleName.STAFF
                ? new List<staff>()
                {
                    new staff()
                    {
                        Fullname = request.Fullname,
                        Phone = request.Phone,
                        Address = request.Address,
                    }
                }
                : null,
            Partner = request.RoleName.ToString() == RoleName.PARTNER
                ? new Partner()
                {
                    Fullname = request.Fullname,
                    Phone = request.Phone,
                    Address = request.Address,
                    Gender = request.Gender,
                }
                : null,
        });

        await Task.WhenAll(new List<Task>() { userFb, userDb });

        return await this._customerRepo.GetCustomerById(id);
    }
}