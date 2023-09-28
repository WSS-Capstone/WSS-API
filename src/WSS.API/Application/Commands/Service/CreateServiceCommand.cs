using WSS.API.Data.Repositories.Service;

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
    private IMapper _mapper;
    private IServiceRepo _serviceRepo;

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
        var service = _mapper.Map<Data.Models.Service>(request);
        service.Id = Guid.NewGuid();
        service.CreateDate = DateTime.Now;
        service.Status = (int?)ServiceStatus.ACTIVE;
        var query = await _serviceRepo.CreateService(service);
        
        var result = this._mapper.Map<ServiceResponse>(query);
        
        return result;
    }
}