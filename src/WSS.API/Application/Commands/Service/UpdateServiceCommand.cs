using WSS.API.Application.Commands.Category;
using WSS.API.Data.Repositories.Account;
using WSS.API.Data.Repositories.Service;
using WSS.API.Infrastructure.Services.Identity;

namespace WSS.API.Application.Commands.Service;

public class UpdateServiceCommand : IRequest<ServiceResponse>
{
    public UpdateServiceCommand(Guid id, CreateServiceCommand command)
    {
        Id = id;
        Name = command.Name;
        Quantity = command.Quantity;
        ImageUrls = command.ImageUrls;
        Categoryid = command.Categoryid;
        Price = command.Price;
        Unit = command.Unit;
        Description = command.Description;
    }

    public Guid Id { get; set; }
    public string? Name { get; set; }
    public int? Quantity { get; set; }
    public string[]? ImageUrls { get; set; }
    public Guid? Categoryid { get; set; }
    public string? Unit { get; set; }
    public double? Price { get; set; }
    public string? Description { get; set; }
}

public class InactiveServiceRequest
{
    public string? Reason { get; set; }
}

public class UpdateServiceCommandHandler : IRequestHandler<UpdateServiceCommand, ServiceResponse>
{
    private IMapper _mapper;
    private IServiceRepo _serviceRepo;
    private readonly IIdentitySvc _identitySvc;
    private readonly IAccountRepo _accountRepo;

    public UpdateServiceCommandHandler(IMapper mapper, IServiceRepo serviceRepo, IIdentitySvc identitySvc,
        IAccountRepo accountRepo)
    {
        _mapper = mapper;
        _serviceRepo = serviceRepo;
        _identitySvc = identitySvc;
        _accountRepo = accountRepo;
    }

    /// <summary>
    /// Handle update service command
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ServiceResponse> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        var user = await this._accountRepo.GetAccounts(a => a.RefId == this._identitySvc.GetUserRefId(),
            new Expression<Func<Data.Models.Account, object>>[]
            {
                a => a.User
            }).FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (user.RoleName != "Owner" && user.RoleName != "Partner")
        {
            throw new Exception("You are not allowed to create service");
        }

        var service = await this._serviceRepo.GetServiceById(request.Id,
            new Expression<Func<Data.Models.Service, object>>[]
            {
                s => s.Category,
                s => s.ServiceImages,
                s => s.CurrentPrices
            });

        if (service == null)
        {
            throw new Exception("service not found");
        }

        service = _mapper.Map(request, service);
        service.CurrentPrices.Add(
            new()
            {
                Price = request.Price,
                CreateDate = DateTime.Now,
                ServiceId = service.Id,
                Id = Guid.NewGuid(),
                DateOfApply = DateTime.Today,
            } 
        );
        service.UpdateDate = DateTime.Now;
        if (request.ImageUrls is
            {
                Length: > 0
            })
        {
            service.CoverUrl = request.ImageUrls?.FirstOrDefault();
            service.ServiceImages = new List<ServiceImage>();
            foreach (var imageUrl in request.ImageUrls)
            {
                service.ServiceImages.Add(new ServiceImage()
                {
                    Id = Guid.NewGuid(),
                    ServiceId = service.Id,
                    ImageUrl = imageUrl
                });
            }
        }

        if (request.Price != null)
        {
            service.CurrentPrices = new List<Data.Models.CurrentPrice>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    ServiceId = service.Id,
                    Price = request.Price,
                    CreateDate = DateTime.Now
                }
            };
        }

        var query = await _serviceRepo.UpdateService(service);

        var result = this._mapper.Map<ServiceResponse>(query);
        result.IsOwnerService = user.RoleName == "Owner";
        return result;
    }
}