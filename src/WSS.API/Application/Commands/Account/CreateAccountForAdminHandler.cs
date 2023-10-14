using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc.Rendering;
using WSS.API.Data.Repositories.Account;
using WSS.API.Data.Repositories.Customer;
using WSS.API.Data.Repositories.Partner;
using WSS.API.Data.Repositories.staff;
using WSS.API.Infrastructure.Config;
using Task = System.Threading.Tasks.Task;

namespace WSS.API.Application.Commands.Account;

public class CreateAccountForAdminCommand : IRequest<Data.Models.Account?>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Fullname { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public int Gender { get; set; }
    public string? ImageUrl { get; set; }
    public Guid? CategoryId { get; set; }
    public RoleEnum RoleName { get; set; }
}

public class CreateAccountForAdminHandler : IRequestHandler<CreateAccountForAdminCommand, Data.Models.Account?>
{
    private IAccountRepo _accountRepo;
    private FirebaseAuth _firebaseAuth;
    private IPartnerRepo _partnerRepo;
    private IStaffRepo _staffRepo;

    public CreateAccountForAdminHandler(IAccountRepo accountRepo, FirebaseAuth firebaseAuth,
        IPartnerRepo partnerRepo, IStaffRepo staffRepo)
    {
        _accountRepo = accountRepo;
        _firebaseAuth = firebaseAuth;
        _partnerRepo = partnerRepo;
        _staffRepo = staffRepo;
    }

    public async Task<Data.Models.Account> Handle(CreateAccountForAdminCommand request,
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
            Status = 0,
            Username = request.Email,
            CreateDate = DateTime.Now
        });

        if (request.RoleName == RoleEnum.Partner)
        {
            await this._partnerRepo.CreatePartner(new Data.Models.Partner()
            {
                Id = id,
                Fullname = request.Fullname,
                DateOfBirth = request.DateOfBirth,
                Phone = request.Phone,
                Address = request.Address,
                Gender = request.Gender,
                ImageUrl = request.ImageUrl,
                CategoryId = request.CategoryId,
                CreateDate = DateTime.Now
            });
        }
        else
        {
            await _staffRepo.CreateStaff(new staff()
            {
                Id = id,
                Fullname = request.Fullname,
                DateOfBirth = request.DateOfBirth,
                Phone = request.Phone,
                Address = request.Address,
                Gender = request.Gender,
                ImageUrl = request.ImageUrl,
                CategoryId = (Guid)request.CategoryId,
                CreateDate = DateTime.Now
            });
        }

        var response = await _accountRepo.GetAccountById(id);
        return response;
    }
}