using WSS.API.Data.Repositories.Account;
using WSS.API.Data.Repositories.Service;
using WSS.API.Infrastructure.Services.Identity;

namespace WSS.API.Application.Commands.Service;

public class ApprovalServiceCommand : IRequest<ServiceResponse>
{
     public ApprovalServiceCommand(Guid id, ApprovalServiceRequest request)
    {
        Id = id;
        Status = request.Status;
        Reason = request.Reason;
    }

    public Guid Id { get; set; }
    public ServiceStatus Status { get; set; }
    public string Reason { get; set; }
}

public class ApprovalServiceRequest
{
    public ServiceStatus Status { get; set; }
    public string Reason { get; set; }
}

public class ApprovalServiceCommandHandler : IRequestHandler<ApprovalServiceCommand, ServiceResponse>
{
    private readonly IAccountRepo _accountRepo;
    private readonly IServiceRepo _serviceRepo;
    private readonly IMapper _mapper;
    private readonly IIdentitySvc _identitySvc;

    public ApprovalServiceCommandHandler(IAccountRepo accountRepo, IServiceRepo serviceRepo, IMapper mapper,
        IIdentitySvc identitySvc)
    {
        _accountRepo = accountRepo;
        _serviceRepo = serviceRepo;
        _mapper = mapper;
        _identitySvc = identitySvc;
    }

    public async Task<ServiceResponse> Handle(ApprovalServiceCommand request, CancellationToken cancellationToken)
    {
        var user = await this._accountRepo.GetAccounts(a => a.RefId == this._identitySvc.GetUserRefId(),
            new Expression<Func<Data.Models.Account, object>>[]
            {
                a => a.User
            }).FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (user.RoleName != "Owner")
        {
            throw new Exception("You are not allowed to create service");
        }

        var service = await this._serviceRepo.GetServiceById(request.Id,
            new Expression<Func<Data.Models.Service, object>>[]
            {
                s => s.Category,
                s => s.ServiceImages
            });

        if (service == null)
        {
            throw new Exception("Service not found");
        }

        service = _mapper.Map(request, service);
        service.UpdateDate = DateTime.Now;
        service.Status = (int?)request.Status;
        service.Reason = request.Reason;
        var query = await _serviceRepo.UpdateService(service);

        var result = this._mapper.Map<ServiceResponse>(query);

        return result;
    }
}