using WSS.API.Data.Repositories.Task;

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
    private readonly ITaskRepo _feedbackRepo;

    public CreateTaskCommandHandler(IMapper mapper, ITaskRepo feedbackRepo)
    {
        _mapper = mapper;
        _feedbackRepo = feedbackRepo;
    }

    public async Task<TaskResponse> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var feedback = _mapper.Map<Data.Models.Task>(request);
        feedback.Id = Guid.NewGuid();
        feedback.CreateDate = DateTime.UtcNow;
        
        feedback = await _feedbackRepo.CreateTask(feedback);
        
        return _mapper.Map<TaskResponse>(feedback);
    }
}