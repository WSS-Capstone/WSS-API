using WSS.API.Data.Repositories.Account;
using WSS.API.Data.Repositories.Service;
using WSS.API.Infrastructure.Services.Identity;
using WSS.API.Infrastructure.Services.Noti;

namespace WSS.API.Application.Commands.Service;

public class StatusServiceCommand : IRequest<ServiceResponse>
{
    public Guid Id { get; set; }
    public ServiceStatus Status { get; set; }
}

public class StatusServiceRequest
{
    public ServiceStatus Status { get; set; }
}

public class StatusServiceCommandHandler : IRequestHandler<StatusServiceCommand, ServiceResponse>
{
    private readonly IAccountRepo _accountRepo;
    private readonly IServiceRepo _serviceRepo;
    private readonly IMapper _mapper;
    private readonly IIdentitySvc _identitySvc;

    public StatusServiceCommandHandler(IAccountRepo accountRepo, IServiceRepo serviceRepo, IMapper mapper,
        IIdentitySvc identitySvc)
    {
        _accountRepo = accountRepo;
        _serviceRepo = serviceRepo;
        _mapper = mapper;
        _identitySvc = identitySvc;
    }

    public async Task<ServiceResponse> Handle(StatusServiceCommand request, CancellationToken cancellationToken)
    {
        var user = await this._accountRepo.GetAccounts(a => a.RefId == this._identitySvc.GetUserRefId(),
            new Expression<Func<Data.Models.Account, object>>[]
            {
                a => a.User
            }).FirstOrDefaultAsync(cancellationToken: cancellationToken);

        // if (user.RoleName != "Owner")
        // {
        //     throw new Exception("You are not allowed to create service");
        // }

        var serviceQuery = this._serviceRepo.GetServices(s => s.Id == request.Id,
            new Expression<Func<Data.Models.Service, object>>[]
            {
                s => s.Category,
                s => s.ServiceImages
            });

        var service = await serviceQuery.FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (service == null || service.CreateBy != user.Id)
        {
            throw new Exception("Service not found");
        }

        if (request.Status == ServiceStatus.Active)
        {
            // send notification to partner
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                { "type", "Service" },
                { "userId", service.CreateBy.ToString() }
            };
            await NotiService.PushNotification.SendMessage(service.CreateBy.ToString(),
                $"Thông báo duyệt dịch vụ.",
                $"Dịch vụ {service.Code} đã được duyệt.", data);
        }

        else if (request.Status == ServiceStatus.Reject)
        {
            // send notification to partner
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                { "type", "Service" },
                { "userId", service.CreateBy.ToString() }
            };
            await NotiService.PushNotification.SendMessage(service.CreateBy.ToString(),
                $"Thông báo từ chối dịch vụ.",
                $"Dịch vụ {service.Code} đã bị từ chối.", data);
        }

        service.Status = (int)request.Status;
        service.UpdateDate = DateTime.Now;

        await this._serviceRepo.UpdateService(service);

        return this._mapper.Map<ServiceResponse>(service);
    }
}