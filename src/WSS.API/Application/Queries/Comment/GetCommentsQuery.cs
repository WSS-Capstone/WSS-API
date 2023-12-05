using WSS.API.Data.Repositories.Comment;

namespace WSS.API.Application.Queries.Comment;

public class GetCommentsQuery : PagingParam<CommentSortCriteria>,
    IRequest<PagingResponseQuery<CommentResponse, CommentSortCriteria>>
{
    public Guid? TaskId { get; set; }
}

public class GetCommentActiveRequest : PagingParam<CommentSortCriteria>
{
    public string? Content { get; set; }
    public Guid? TaskId { get; set; }
}

public enum CommentSortCriteria
{
    CreateDate
}

public class GetCommentsQueryHandler : IRequestHandler<GetCommentsQuery,
    PagingResponseQuery<CommentResponse, CommentSortCriteria>>
{
    private readonly IMapper _mapper;
    private readonly ICommentRepo _commentRepo;

    public GetCommentsQueryHandler(IMapper mapper, ICommentRepo commentRepo)
    {
        _mapper = mapper;
        _commentRepo = commentRepo;
    }

    /// <inheritdoc />
    public async Task<PagingResponseQuery<CommentResponse, CommentSortCriteria>> Handle(GetCommentsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _commentRepo.GetComments(null, new Expression<Func<Data.Models.Comment, object>>[]
        {
            c => c.Task,
            c => c.CreateByNavigation,
        });
        
        if(request.TaskId != null)
        {
            query = query.Where(c => c.TaskId == request.TaskId);
        }
        var total = await query.CountAsync(cancellationToken: cancellationToken);

        query = query.GetWithSorting(request.SortKey.ToString(), request.SortOrder);

        query = query.GetWithPaging(request.Page, request.PageSize);
        
        var list = await query.ToListAsync(cancellationToken: cancellationToken);

        var result = this._mapper.Map<List<CommentResponse>>(list);

        return new PagingResponseQuery<CommentResponse, CommentSortCriteria>(request, result.AsQueryable(), total);
    }
}