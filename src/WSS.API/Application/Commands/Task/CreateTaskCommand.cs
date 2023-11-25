using WSS.API.Data.Repositories.Task;
using WSS.API.Infrastructure.Utilities;
using TaskStatus = WSS.API.Application.Models.ViewModels.TaskStatus;

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
        task.OrderDetailId = (Guid)request.OrderDetailId;
        task.Id = Guid.NewGuid();
        task.Code = GenCode.NextId(code);
        task.CreateDate = DateTime.UtcNow;
        task.Status = (int)TaskStatus.TO_DO;
        
        task = await _taskRepo.CreateTask(task);
        
        return _mapper.Map<TaskResponse>(task);
    }
}