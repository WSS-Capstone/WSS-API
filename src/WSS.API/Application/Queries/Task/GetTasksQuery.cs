using WSS.API.Data.Repositories.Task;

namespace WSS.API.Application.Queries.Task;

public class GetTasksQuery: PagingParam<TaskSortCriteria>, IRequest<PagingResponseQuery<TaskResponse, TaskSortCriteria>>
{
    public Guid? UserId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class GetTaskOwnerRequest : PagingParam<TaskSortCriteria>{}

public enum TaskSortCriteria
{
    Id,
    TaskName,
    Content,
    StartDate,
    EndDate,
    CreateDate
}

public class GetTasksQueryHandler: IRequestHandler<GetTasksQuery, PagingResponseQuery<TaskResponse, TaskSortCriteria>>
{
    private IMapper _mapper;
    private ITaskRepo _categoryRepo;

    public GetTasksQueryHandler(IMapper mapper, ITaskRepo categoryRepo)
    {
        _mapper = mapper;
        _categoryRepo = categoryRepo;
    }

    public async Task<PagingResponseQuery<TaskResponse, TaskSortCriteria>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
    {
        var query = _categoryRepo.GetTasks(null, new Expression<Func<Data.Models.Task, object>>[]
        {
            t => t.OrderDetail,
            t => t.Partner,
            t => t.Staff,
            t => t.Comments
        });

        query = query.Include(t => t.OrderDetail.Service);
        query = query.Include(t => t.OrderDetail.Order);

        if (request.UserId != null)
        {
            query = query.Where(x => x.StaffId == request.UserId || x.PartnerId == request.UserId);
        }

        if (request.StartDate != null && request.EndDate != null)
        {
            query = query.Where(t => t.StartDate >= request.StartDate && t.EndDate <= request.EndDate);
        }
        
        var total = await query.CountAsync(cancellationToken: cancellationToken);
        
        query = query.GetWithSorting(request.SortKey.ToString(), request.SortOrder);
        
        query = query.GetWithPaging(request.Page, request.PageSize);
        var list = await query.ToListAsync(cancellationToken: cancellationToken);
        var result = this._mapper.ProjectTo<TaskResponse>(list.AsQueryable());

        return new PagingResponseQuery<TaskResponse, TaskSortCriteria>(request, result, total);
    }
}