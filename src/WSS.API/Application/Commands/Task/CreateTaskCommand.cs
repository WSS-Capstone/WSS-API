using WSS.API.Data.Repositories.Task;
using WSS.API.Infrastructure.Utilities;

namespace WSS.API.Application.Commands.Task;

public class CreateTaskCommand : IRequest<TaskResponse>
{
    public Guid? StaffId { get; set; }
    public Guid? PartnerId { get; set; }
    public Guid? OrderDetailId { get; set; }
    public string? TaskName { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? ImageEvidence { get; set; }
}

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskResponse>
{
    private readonly IMapper _mapper;
    private readonly ITaskRepo _taskRepo;

    public CreateTaskCommandHandler(IMapper mapper, ITaskRepo taskRepo)
    {
        _mapper = mapper;
        _taskRepo = taskRepo;
    }

    public async Task<TaskResponse> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var code = await _taskRepo.GetTasks().OrderByDescending(x => x.Code).Select(x => x.Code)
            .FirstOrDefaultAsync(cancellationToken);
        var task = _mapper.Map<Data.Models.Task>(request);
        task.TaskOrderDetails = new List<TaskOrderDetail>()
        {
            new TaskOrderDetail()
            {
                OrderDetailId = (Guid)request.OrderDetailId,
                
            }
        };
        task.Id = Guid.NewGuid();
        task.Code = GenCode.NextId(code);
        task.CreateDate = DateTime.UtcNow;
        
        task = await _taskRepo.CreateTask(task);

        var result = _taskRepo.GetTasks(t => t.Id == task.Id, new Expression<Func<Data.Models.Task, object>>[]
        {
            t => t.TaskOrderDetails,
            t => t.Partner,
            t => t.Staff,
            t => t.Comments,
            t => t.CreateByNavigation
        });
        
        result = result.Include(t => t.OrderDetail.Service);
        result = result.Include(t => t.OrderDetail.Order);
        result = result.Include(t => t.OrderDetail.Order).ThenInclude(o => o.WeddingInformation);
        result = result.Include(t => t.OrderDetail.Order).ThenInclude(o => o.Customer);
        result = result.Include(t => t.OrderDetail.Order).ThenInclude(o => o.Combo);
        result = result.Include(t => t.OrderDetail.Order).ThenInclude(o => o.Voucher);
        result = result.Include(t => t.TaskOrderDetails).ThenInclude(k => k.OrderDetail).ThenInclude(o => o.Order);
        result = result.Include(t => t.TaskOrderDetails).ThenInclude(k => k.OrderDetail).ThenInclude(o => o.Service);


        var newTask =await  result.FirstOrDefaultAsync(cancellationToken: cancellationToken);
        var map = _mapper.Map<TaskResponse>(newTask);
        map.OrderDetails = map.TaskOrderDetails.Select(x => _mapper.Map<OrderDetailResponse>(x.OrderDetail))
            .ToList();
        map.TaskOrderDetails.Clear();
        
        return _mapper.Map<TaskResponse>(newTask);
    }
}