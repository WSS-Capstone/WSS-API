using WSS.API.Data.Repositories.Feedback;

namespace WSS.API.Application.Queries.Feedback;

public class GetFeedbackByIdQuery : IRequest<FeedbackResponse>
{
    public GetFeedbackByIdQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}
public class GetFeedbackByIdQueryHandler : IRequestHandler<GetFeedbackByIdQuery, FeedbackResponse>
{
    private IMapper _mapper;
    private IFeedbackRepo _repo;

    public GetFeedbackByIdQueryHandler(IMapper mapper, IFeedbackRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<FeedbackResponse> Handle(GetFeedbackByIdQuery request, CancellationToken cancellationToken)
    {
        var query = await _repo.GetFeedbackById(request.Id);
        var result = this._mapper.Map<FeedbackResponse>(query);

        return result;
    }
}