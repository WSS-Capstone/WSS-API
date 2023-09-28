using WSS.API.Application.Commands.Category;
using WSS.API.Data.Repositories.Service;

namespace WSS.API.Application.Commands.Service;

public class UpdateServiceCommand : IRequest<ServiceResponse>
{
    public UpdateServiceCommand(Guid id , CreateServiceCommand command)
    {
        Id = id;
        Name = command.Name;
        Quantity = command.Quantity;
        CoverUrl = command.CoverUrl;
        Categoryid = command.Categoryid;
        OwnerId = command.OwnerId;
        Description = command.Description;
    }

    public Guid Id { get; set; }
    public string? Name { get; set; }
    public int? Quantity { get; set; }
    public string? CoverUrl { get; set; }
    public Guid? Categoryid { get; set; }
    public Guid? OwnerId { get; set; }
    public string? Description { get; set; }
}

public class UpdateServiceCommandHandler : IRequestHandler<UpdateServiceCommand, ServiceResponse>
{
    private IMapper _mapper;
    private IServiceRepo _serviceRepo;

    public UpdateServiceCommandHandler(IMapper mapper, IServiceRepo serviceRepo)
    {
        _mapper = mapper;
        _serviceRepo = serviceRepo;
    }

    /// <summary>
    /// Handle update service command
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ServiceResponse> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        var service = _mapper.Map<Data.Models.Service>(request);
        service.UpdateDate = DateTime.Now;
        var query = await _serviceRepo.UpdateService(service);
        
        var result = this._mapper.Map<ServiceResponse>(query);
        
        return result;
    }
}