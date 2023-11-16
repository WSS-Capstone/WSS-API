using WSS.API.Data.Repositories.Task;

namespace WSS.API.Application.Queries.Task;

public class GetTaskByIdQuery : IRequest<TaskResponse>
{
    public GetTaskByIdQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}
public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskResponse>
{
    private IMapper _mapper;
    private ITaskRepo _repo;

    public GetTaskByIdQueryHandler(IMapper mapper, ITaskRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<TaskResponse> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var query =  _repo.GetTasks(t => t.Id == request.Id, new Expression<Func<Data.Models.Task, object>>[]
        {
            t => t.OrderDetail,
            t => t.Partner,
            t => t.Staff,
            t => t.Comments
        });
        
        query = query.Include(t => t.OrderDetail.Service).ThenInclude(s => s.ServiceImages);
        query = query.Include(t => t.OrderDetail.Order);
        query = query.Include(t => t.OrderDetail.Order).ThenInclude(o => o.WeddingInformation);
        query = query.Include(t => t.OrderDetail.Order).ThenInclude(o => o.Customer).ThenInclude(a => a.IdNavigation);
        query = query.Include(t => t.OrderDetail.Order).ThenInclude(o => o.Combo);
        query = query.Include(t => t.OrderDetail.Order).ThenInclude(o => o.Voucher);
        query = query.Include(t => t.TaskOrderDetails).ThenInclude(k => k.OrderDetail).ThenInclude(o => o.Order);
        query = query.Include(t => t.TaskOrderDetails).ThenInclude(k => k.OrderDetail).ThenInclude(o => o.Service);

        
        var task = await query.FirstOrDefaultAsync(cancellationToken);
        var result = this._mapper.Map<TaskResponse>(task);
        result.OrderDetails = result.TaskOrderDetails.Select(x => _mapper.Map<OrderDetailResponse>(x.OrderDetail))
            .ToList();
        result.TaskOrderDetails.Clear();
        return result;
    }
}