using WSS.API.Data.Repositories.Comment;

namespace WSS.API.Application.Commands.Comment;

public class UpdateCommentCommand : IRequest<CommentResponse>
{
    public UpdateCommentCommand(Guid id, UpdateCommentRequest command)
    {
        Id = id;
        Content = command.Content;
    }

    public Guid Id { get; set; }
    public string? Content { get; set; }
}

public class UpdateCommentRequest
{
    public Guid Id { get; set; }
    public string? Content { get; set; }
}


public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, CommentResponse>
{
    private IMapper _mapper;
    private ICommentRepo _commentRepo;

    public UpdateCommentCommandHandler(IMapper mapper, ICommentRepo commentRepo)
    {
        _mapper = mapper;
        _commentRepo = commentRepo;
    }

    public async Task<CommentResponse> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await this._commentRepo.GetCommentById(request.Id);
        if (comment == null)
        {
            throw new Exception("Comment not found");
        }
        
        comment = _mapper.Map(request, comment);
        var query = await _commentRepo.UpdateComment(comment);
        
        var result = this._mapper.Map<CommentResponse>(query);
        
        return result;
    }
}