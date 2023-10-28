using WSS.API.Data.Repositories.Comment;

namespace WSS.API.Application.Queries.Comment;

public class GetCommentByIdQuery : IRequest<CommentResponse>
{
    public GetCommentByIdQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}

public class GetCommentByIdQueryHandler : IRequestHandler<GetCommentByIdQuery, CommentResponse>
{
    private IMapper _mapper;
    private ICommentRepo _commentRepo;

    public GetCommentByIdQueryHandler(IMapper mapper, ICommentRepo commentRepo)
    {
        _mapper = mapper;
        _commentRepo = commentRepo;
    }

    /// <inheritdoc />
    public async Task<CommentResponse> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await this._commentRepo.GetCommentById(request.Id, new Expression<Func<Data.Models.Comment, object>>[]
        {
            c => c.Task
        });

        return this._mapper.Map<CommentResponse>(result);
    }
}