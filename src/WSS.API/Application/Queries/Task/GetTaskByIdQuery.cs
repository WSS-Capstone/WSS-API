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
        var query = await _repo.GetTaskById(request.Id);
        var result = this._mapper.Map<TaskResponse>(query);

        return result;
    }
}