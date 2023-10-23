using WSS.API.Data.Repositories.Service;
using WSS.API.Infrastructure.Utilities;

namespace WSS.API.Application.Commands.Service;

public class CreateServiceCommand : IRequest<ServiceResponse>
{
    public string? Name { get; set; }
    public int? Quantity { get; set; }
    public string? CoverUrl { get; set; }
    public Guid? Categoryid { get; set; }
    public Guid? OwnerId { get; set; }
    public string? Description { get; set; }
}

public class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand, ServiceResponse>
{
    private readonly IMapper _mapper;
    private readonly IServiceRepo _serviceRepo;

    public CreateServiceCommandHandler(IMapper mapper, IServiceRepo serviceRepo)
    {
        _mapper = mapper;
        _serviceRepo = serviceRepo;
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
        service.Id = Guid.NewGuid();
        service.Code = GenCode.NextId(code);
        service.CreateDate = DateTime.Now;
        service.Status = (int?)ServiceStatus.Active;
        var query = await _serviceRepo.CreateService(service);
        query = await this._serviceRepo.GetServiceById(query.Id, new Expression<Func<Data.Models.Service, object>>[]
        {
            s => s.Category,
            s => s.CurrentPrices
        });

        var result = this._mapper.Map<ServiceResponse>(query);
        
        return result;
    }
}