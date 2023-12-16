using WSS.API.Data.Repositories.Task;
using TaskStatus = WSS.API.Application.Models.ViewModels.TaskStatus;

namespace WSS.API.Application.Queries.Task;

public class GetTasksQuery : PagingParam<TaskSortCriteria>,
    IRequest<PagingResponseQuery<TaskResponse, TaskSortCriteria>>
{
    public Guid? UserId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? TaskName { get; set; }
    public bool? OfPartner { get; set; } = null;

    public DateTime? StartDateFrom { get; set; }
    public DateTime? StartDateTo { get; set; }

    public TaskStatus[]? Status { get; set; } = new[]
        { TaskStatus.EXPECTED, TaskStatus.TO_DO, TaskStatus.DONE, TaskStatus.IN_PROGRESS };
}

public class GetTaskOwnerRequest : PagingParam<TaskSortCriteria>
{
    public string? TaskName { get; set; }
    public DateTime? StartDateFrom { get; set; }
    public DateTime? StartDateTo { get; set; }

    public TaskStatus[]? Status { get; set; } = new[]
        { TaskStatus.EXPECTED, TaskStatus.TO_DO, TaskStatus.DONE, TaskStatus.IN_PROGRESS };
}

public enum TaskSortCriteria
{
    Id,
    Code,
    TaskName,
    Content,
    StartDate,
    EndDate,
    CreateDate,
    Status
}

public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, PagingResponseQuery<TaskResponse, TaskSortCriteria>>
{
    private IMapper _mapper;
    private ITaskRepo _categoryRepo;

    public GetTasksQueryHandler(IMapper mapper, ITaskRepo categoryRepo)
    {
        _mapper = mapper;
        _categoryRepo = categoryRepo;
    }

    public async Task<PagingResponseQuery<TaskResponse, TaskSortCriteria>> Handle(GetTasksQuery request,
        CancellationToken cancellationToken)
    {
        var query = _categoryRepo.GetTasks(null, new Expression<Func<Data.Models.Task, object>>[]
        {
            t => t.OrderDetail,
            t => t.Partner,
            t => t.Staff,
            // t => t.Comments,
            t => t.CreateByNavigation,
        });

        query = query.Include(t => t.OrderDetail).ThenInclude(k => k.Order).ThenInclude(o => o.Customer);
        query = query.Include(t => t.OrderDetail).ThenInclude(k => k.Service);
        query = query.Include(t => t.Comments).ThenInclude(k => k.CreateByNavigation);
        
        if (request.UserId != null)
        {
            query = query.Where(x => x.StaffId == request.UserId || x.PartnerId == request.UserId);
        }

        if (request.StartDate != null && request.EndDate != null)
        {
            query = query.Where(t =>
                t.StartDate >= request.StartDate &&
                t.EndDate <= request.EndDate);
        }

        if (request.TaskName != null)
        {
            query = query.Where(t => t.TaskName.Contains(request.TaskName));
        }
        
        if (request.StartDateFrom != null)
        {
            query = query.Where(t => t.StartDate != null && t.StartDate.Value.Date >= request.StartDateFrom.Value.Date);
        }

        if (request.StartDateTo != null)
        {
            query = query.Where(t => t.StartDate != null && t.StartDate.Value.Date <= request.StartDateTo.Value.Date);
        }

        if (request.Status != null && request.Status.Length > 0)
        {
            query = query.Where(t => t.Status != null && request.Status.Contains((TaskStatus)t.Status));
        }

        if (request.OfPartner != null)
        {
            if ((bool)request.OfPartner)
            {
                query = query.Where(t => t.PartnerId != null && t.StaffId == null);
            }
            else
            {
                query = query.Where(t =>
                    t.StaffId != null && t.PartnerId == null || t.StaffId == null && t.PartnerId == null);
            }
        }


        var total = await query.CountAsync(cancellationToken: cancellationToken);

        query = query.GetWithSorting(request.SortKey.ToString(), request.SortOrder);

        query = query.GetWithPaging(request.Page, request.PageSize);
        var list = await query.ToListAsync(cancellationToken: cancellationToken);
        
        var result = this._mapper.Map<List<TaskResponse>>(list);
        result.ForEach(t =>
        {
            t.OrderDetail.Tasks.Clear();
            // t.OrderDetails = t.TaskOrderDetails.Select(x => _mapper.Map<OrderDetailResponse>(x.OrderDetail))
            //     .ToList();
            // t.TaskOrderDetails.Clear();
            // t.Order?.OrderDetails.Clear();
            // t.OrderDetail.Order = null;
            // t.OrderDetail.Service = null;
        });
        return new PagingResponseQuery<TaskResponse, TaskSortCriteria>(request, result.AsQueryable(), total);
    }
}