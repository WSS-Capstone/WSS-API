using WSS.API.Data.Repositories.Service;

namespace WSS.API.Application.Commands.Service;

public class UpdateServiceStatusCommand : IRequest<ServiceResponse>
{
    public Guid Id { get; set; }
    public ServiceStatus Status { get; set; }
}

public class UpdateServiceStatusCommandHandler : IRequestHandler<UpdateServiceStatusCommand, ServiceResponse>
{
    private readonly IMapper _mapper;
    private readonly IServiceRepo _serviceRepo;

    public UpdateServiceStatusCommandHandler(IMapper mapper, IServiceRepo serviceRepo)
    {
        _mapper = mapper;
        _serviceRepo = serviceRepo;
    }

    public async Task<ServiceResponse> Handle(UpdateServiceStatusCommand request,
        CancellationToken cancellationToken)
    {
        var service = await _serviceRepo.GetServiceById(request.Id);
        if (service == null)
        {
            throw new Exception("Service not found");
        }

        service.Status = (int)request.Status;
        service.CreateDate = DateTime.Now;
        await _serviceRepo.UpdateService(service);
        return _mapper.Map<ServiceResponse>(service);
    }
}