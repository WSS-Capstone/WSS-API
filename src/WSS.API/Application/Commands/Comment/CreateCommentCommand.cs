using WSS.API.Data.Repositories.Account;
using WSS.API.Data.Repositories.Comment;
using WSS.API.Infrastructure.Services.Identity;
using WSS.API.Infrastructure.Utilities;

namespace WSS.API.Application.Commands.Comment;

public class CreateCommentCommand : IRequest<CommentResponse>
{
    public Guid? TaskId { get; set; }
    public string? Content { get; set; }

}

public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, CommentResponse>
{
    private readonly IMapper _mapper;
    private readonly ICommentRepo _commentRepo;
    private readonly IAccountRepo _accountRepo;
    private readonly IIdentitySvc _identitySvc;

    public CreateCommentCommandHandler(IMapper mapper, ICommentRepo commentRepo, IIdentitySvc identitySvc, IAccountRepo accountRepo)
    {
        _mapper = mapper;
        _commentRepo = commentRepo;
        _identitySvc = identitySvc;
        _accountRepo = accountRepo;
    }

    public async Task<CommentResponse> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var user = await this._accountRepo.GetAccounts(a => a.RefId == this._identitySvc.GetUserRefId(),
            new Expression<Func<Data.Models.Account, object>>[]
            {
                a => a.User
            }).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        
        var newId = Guid.NewGuid();
        var comment = _mapper.Map<Data.Models.Comment>(request);
        comment.Id = newId;
        comment.CreateBy = user.Id;
        comment.CreateDate = DateTime.UtcNow;
        
        var query = await _commentRepo.CreateComment(comment);

        var result = this._mapper.Map<CommentResponse>(query);

        return result;
    }
}