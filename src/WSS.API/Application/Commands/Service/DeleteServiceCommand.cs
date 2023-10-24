using WSS.API.Data.Repositories.Service;

namespace WSS.API.Application.Commands.Service;

public class DeleteServiceCommand : IRequest<ServiceResponse>
{
    public DeleteServiceCommand(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}

public class DeleteServiceCommandHandler : IRequestHandler<DeleteServiceCommand, ServiceResponse>
{
    private IMapper _mapper;
    private IServiceRepo _serviceRepo;

    public DeleteServiceCommandHandler(IMapper mapper, IServiceRepo serviceRepo)
    {
        _mapper = mapper;
        _serviceRepo = serviceRepo;
    }

    /// <summary>
    /// Handle delete service command
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ServiceResponse> Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
    {
       var service = await _serviceRepo.GetServiceById(request.Id);
       
       if (service == null)
       {
           throw new Exception("Service not found");
       }

       service.Status = (int?)ServiceStatus.InActive;
       
       var query = await _serviceRepo.UpdateService(service);
       
       var result = this._mapper.Map<ServiceResponse>(query);

       return result;
    }
}