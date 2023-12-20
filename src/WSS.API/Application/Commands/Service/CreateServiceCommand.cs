using WSS.API.Data.Repositories.Account;
using WSS.API.Data.Repositories.Notification;
using WSS.API.Data.Repositories.Service;
using WSS.API.Data.Repositories.User;
using WSS.API.Infrastructure.Config;
using WSS.API.Infrastructure.Services.Identity;
using WSS.API.Infrastructure.Services.Noti;
using WSS.API.Infrastructure.Utilities;

namespace WSS.API.Application.Commands.Service;

public class CreateServiceCommand : IRequest<ServiceResponse>
{
    public string? Name { get; set; }
    public int? Quantity { get; set; }
    public string[]? ImageUrls { get; set; }
    public Guid? Categoryid { get; set; }
    public string? Unit { get; set; }
    public double? Price { get; set; }
    public string? Description { get; set; }
}

public class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand, ServiceResponse>
{
    private readonly IMapper _mapper;
    private readonly IServiceRepo _serviceRepo;
    private readonly IIdentitySvc _identitySvc;
    private readonly IAccountRepo _accountRepo;
    private readonly INotificationRepo _notificationRepo;

    public CreateServiceCommandHandler(IMapper mapper, IServiceRepo serviceRepo, IIdentitySvc identitySvc,
        IAccountRepo accountRepo, INotificationRepo notificationRepo)
    {
        _mapper = mapper;
        _serviceRepo = serviceRepo;
        _identitySvc = identitySvc;
        _accountRepo = accountRepo;
        _notificationRepo = notificationRepo;
    }

    /// <summary>
    /// Handle create service command
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ServiceResponse> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
    {
        var code = await _serviceRepo.GetServices().OrderByDescending(x => x.Code).Select(x => x.Code)
            .FirstOrDefaultAsync(cancellationToken);
        var service = _mapper.Map<Data.Models.Service>(request);

        var user = await this._accountRepo.GetAccounts(a => a.RefId == this._identitySvc.GetUserRefId(),
            new Expression<Func<Data.Models.Account, object>>[]
            {
                a => a.User
            }).FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (user.RoleName != "Owner" && user.RoleName != "Partner")
        {
            throw new Exception("You are not allowed to create service");
        }

        var owner = await this._accountRepo.GetAccounts(a => a.RoleName == RoleName.OWNER).FirstOrDefaultAsync();
        
        service.Id = Guid.NewGuid();
        service.Code = GenCode.NextId(code, "S");
        service.CreateDate = DateTime.Now;

        service.Status = (int?)(user.RoleName == "Owner" ? ServiceStatus.Active : ServiceStatus.Pending);
        service.CreateBy = user.Id;
        if (request.ImageUrls is { Length: > 0 })
        {
            service.CoverUrl = request.ImageUrls?.FirstOrDefault();
            service.ServiceImages = new List<ServiceImage>();
            foreach (var imageUrl in request.ImageUrls)
            {
                service.ServiceImages.Add(new ServiceImage()
                {
                    Id = Guid.NewGuid(),
                    ServiceId = service.Id,
                    ImageUrl = imageUrl,
                });
            }
        }

        service.CurrentPrices = new List<Data.Models.CurrentPrice>()
        {
            new()
            {
                Id = Guid.NewGuid(),
                DateOfApply = DateTime.Now,
                ServiceId = service.Id,
                Price = request.Price,
                CreateDate = DateTime.Now
            }
        };
        var query = await _serviceRepo.CreateService(service);
        query = await this._serviceRepo.GetServiceById(query.Id, new Expression<Func<Data.Models.Service, object>>[]
        {
            s => s.Category,
            s => s.CurrentPrices,
            s => s.ServiceImages
        });
        
        if (user.RoleName == "Partner")
        {
            // send notification to partner
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                { "type", "Task" },
                { "userId", owner.Id.ToString() }
            };
            await NotiService.PushNotification.SendMessage(owner.Id.ToString(),
                $"Thông báo dịch vụ.",
                $"Bạn có 1 dịch vụ {query.Code} của đối tác cần được duyệt.", data);
            
            var notification = new Data.Models.Notification()
            {
                Title = "Thông báo dịch vụ.",
                Content = $"Bạn có 1 dịch vụ {query.Code} đối tác mới cần được duyệt.",
                UserId = owner.Id
            };
            await _notificationRepo.CreateNotification(notification);
        }

        var result = this._mapper.Map<ServiceResponse>(query);

        result.IsOwnerService = user.RoleName == "Owner";

        return result;
    }
}