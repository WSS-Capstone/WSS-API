using WSS.API.Data.Repositories.Comment;

namespace WSS.API.Application.Commands.Comment;

public class DeleteCommentCommand : IRequest<CommentResponse>
{
    public DeleteCommentCommand(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}


public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, CommentResponse>
{
    private IMapper _mapper;
    private ICommentRepo _commentRepo;

    public DeleteCommentCommandHandler(IMapper mapper, ICommentRepo commentRepo)
    {
        _mapper = mapper;
        _commentRepo = commentRepo;
    }

    public async Task<CommentResponse> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await this._commentRepo.GetCommentById(request.Id);
        
        if (comment == null)
        {
            throw new Exception("Comment not found");
        }
        
        var query = await _commentRepo.DeleteComment(comment);
        
        var result = this._mapper.Map<CommentResponse>(query);
        
        return result;
    }
}