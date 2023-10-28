using WSS.API.Data.Repositories.Task;
using TaskStatus = WSS.API.Application.Models.ViewModels.TaskStatus;

namespace WSS.API.Application.Commands.Task;

public class UpdateTaskCommand : IRequest<TaskResponse>
{
    public UpdateTaskCommand()
    {
    }

    public UpdateTaskCommand(Guid id, UpdateTaskRequest request)
    {
        Id = id;
        OrderDetailId = request.OrderDetailId;
        UserId = request.UserId;
        Status = request.Status;
    }
    
    public Guid Id { get; set; }
    public Guid? OrderDetailId { get; set; }
    public Guid? UserId { get; set; }
    public int? Status { get; set; }
}

public class UpdateStatusTaskRequest
{
    public Guid Id { get; set; }
    public TaskStatus? Status { get; set; }
}

public class UpdateTaskRequest
{
    public Guid Id { get; set; }
    public Guid? OrderDetailId { get; set; }
    public Guid? UserId { get; set; }
    public int? Status { get; set; }
}

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, TaskResponse>
{
    private IMapper _mapper;
    private ITaskRepo _repo;

    public UpdateTaskCommandHandler(IMapper mapper, ITaskRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<TaskResponse> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var feedback = await _repo.GetTaskById(request.Id);
        if (feedback == null)
        {
            throw new Exception("Task not found");
        }
       
        feedback = this._mapper.Map(request, feedback, o =>
        {
            o.BeforeMap((o1, o2) =>
            {
                o1.Status = o1.Status ?? o2.Status;
                o1.UserId = o1.UserId ?? o2.PartnerId ?? o2.StaffId;
                o1.Id = o2.Id;
                o1.OrderDetailId = o1.OrderDetailId ?? o2.OrderDetailId;
        
            });
        });
        
        await _repo.UpdateTask(feedback);
        var result = this._mapper.Map<TaskResponse>(feedback);

        return result;
    }
}