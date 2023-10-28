using WSS.API.Data.Repositories.Feedback;

namespace WSS.API.Application.Queries.Feedback;

public class GetFeedbacksQuery : PagingParam<FeedbackSortCriteria>,
    IRequest<PagingResponseQuery<FeedbackResponse, FeedbackSortCriteria>>
{
}

public enum FeedbackSortCriteria
{
    Id,
    Content,
    Rating,
    Status,
    CreateDate
}

public class GetFeedbacksQueryHandler : IRequestHandler<GetFeedbacksQuery,
    PagingResponseQuery<FeedbackResponse, FeedbackSortCriteria>>
{
    private IMapper _mapper;
    private IFeedbackRepo _repo;

    public GetFeedbacksQueryHandler(IMapper mapper, IFeedbackRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<PagingResponseQuery<FeedbackResponse, FeedbackSortCriteria>> Handle(GetFeedbacksQuery request,
        CancellationToken cancellationToken)
    {
        var query = _repo.GetFeedbacks(null, new Expression<Func<Data.Models.Feedback, object>>[]
        {
            f => f.OrderDetail,
            f => f.CreateByNavigation
        });

        query = query.Include(l => l.OrderDetail.Service);
        var total = await query.CountAsync(cancellationToken: cancellationToken);

        query = query.GetWithSorting(request.SortKey.ToString(), request.SortOrder);

        query = query.GetWithPaging(request.Page, request.PageSize);
        var groupedFeedback = await query
            .GroupBy(feedback => feedback.Rating)
            .Select(group => new
            {
                Rating = group.Key,
                Feedbacks = group.ToList()
            })
            .ToListAsync();

        var result = groupedFeedback
            .SelectMany(group => group.Feedbacks).AsQueryable()
            .Select(feedback => this._mapper.Map<FeedbackResponse>(feedback));

        return new PagingResponseQuery<FeedbackResponse, FeedbackSortCriteria>(request, result, total);
    }
}