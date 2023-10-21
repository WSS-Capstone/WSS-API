using WSS.API.Data.Repositories.Task;
using WSS.API.Infrastructure.Utilities;

namespace WSS.API.Application.Commands.Task;

public class CreateTaskCommand : IRequest<TaskResponse>
{
    public string? Content { get; set; }
    public int? Rating { get; set; }
    public Guid? OrderDetailId { get; set; }
    public Guid? UserId { get; set; }
    public int? Status { get; set; }
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
        task.Id = Guid.NewGuid();
        task.Code = GenCode.NextId(code);
        task.CreateDate = DateTime.UtcNow;
        
        task = await _taskRepo.CreateTask(task);
        
        return _mapper.Map<TaskResponse>(task);
    }
}