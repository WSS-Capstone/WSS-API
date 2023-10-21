using WSS.API.Data.Repositories.Task;

namespace WSS.API.Application.Commands.Task;

public class UpdateTaskCommand : IRequest<TaskResponse>
{
    public UpdateTaskCommand(Guid id, UpdateTaskRequest request)
    {
        Id = id;
        Content = request.Content;
        Rating = request.Rating;
        OrderDetailId = request.OrderDetailId;
        UserId = request.UserId;
        Status = request.Status;
    }
    
    public Guid Id { get; set; }
    public string? Content { get; set; }
    public int? Rating { get; set; }
    public Guid? OrderDetailId { get; set; }
    public Guid? UserId { get; set; }
    public int? Status { get; set; }
}

public class UpdateTaskRequest
{
    public Guid Id { get; set; }
    public string? Content { get; set; }
    public int? Rating { get; set; }
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
       
        feedback = this._mapper.Map(request, feedback);
        
        await _repo.UpdateTask(feedback);
        var result = this._mapper.Map<TaskResponse>(feedback);

        return result;
    }
}