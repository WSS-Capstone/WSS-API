using WSS.API.Data.Repositories.Service;
using WSS.API.Infrastructure.Config;

namespace WSS.API.Application.Queries.Service;

public class GetServiceByIdQuery : IRequest<ServiceResponse>
{
    public GetServiceByIdQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}

public class GetServiceByIdQueryHandler : IRequestHandler<GetServiceByIdQuery, ServiceResponse>
{
    private IMapper _mapper;
    private IServiceRepo _repo;

    public GetServiceByIdQueryHandler(IMapper mapper, IServiceRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<ServiceResponse> Handle(GetServiceByIdQuery request, CancellationToken cancellationToken)
    {
        var query = _repo.GetServices(s => s.Id == request.Id, new Expression<Func<Data.Models.Service, object>>[]
        {
            s => s.Category,
            s => s.CurrentPrices,
            s => s.ServiceImages,
            s => s.OrderDetails.Select(o => o.Order),
            S => S.OrderDetails.Select(o => o.Feedbacks),
            s => s.CreateByNavigation,
        });
        
        query = query.Include(s => s.OrderDetails)
            .ThenInclude(c => c.Order).ThenInclude(s => s.PartnerPaymentHistories);

        query = query
            .Include(s => s.Category)
            .ThenInclude(c => c.Commision);
        query = query.Include(s => s.ComboServices)
            .ThenInclude(c => c.Combo);

        if (query == null)
        {
            throw new Exception("Service not found");
        }

        var service = await query.FirstOrDefaultAsync(cancellationToken: cancellationToken);

        var result = this._mapper.Map<ServiceResponse>(service);
        // result.IsOwnerService = result.CreateByNavigation.RoleName == RoleName.OWNER;
        result.ComboServices.Clear();
        // result.ComboServices.ForEach(cb => cb.Combo?.ComboServices?.Clear());
        return result;
    }
}