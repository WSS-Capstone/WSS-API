using WSS.API.Data.Repositories.Account;
using WSS.API.Data.Repositories.Comment;
using WSS.API.Data.Repositories.Task;
using WSS.API.Infrastructure.Services.Identity;
using WSS.API.Infrastructure.Services.Noti;
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
    private readonly ITaskRepo _taskRepo;

    public CreateCommentCommandHandler(IMapper mapper, ICommentRepo commentRepo, IIdentitySvc identitySvc, IAccountRepo accountRepo, ITaskRepo taskRepo)
    {
        _mapper = mapper;
        _commentRepo = commentRepo;
        _identitySvc = identitySvc;
        _accountRepo = accountRepo;
        _taskRepo = taskRepo;
    }

    public async Task<CommentResponse> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        // var user = await this._accountRepo.GetAccounts(a => a.RefId == this._identitySvc.GetUserRefId(),
        //     new Expression<Func<Data.Models.Account, object>>[]
        //     {
        //         a => a.User
        //     }).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        //
        var newId = Guid.NewGuid();
        var comment = _mapper.Map<Data.Models.Comment>(request);
        comment.Id = newId;
        comment.CreateBy = await this._identitySvc.GetUserId();
        comment.CreateDate = DateTime.UtcNow;
        // select user by task id
        var task = await this._taskRepo.GetTasks(t => t.Id == request.TaskId).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (task == null)
        {
            throw new Exception("Task not found");
        }
        // send notification to user
        Dictionary<string, string> data1 = new Dictionary<string, string>()
        {
            { "type", "Comment" },
            { "userId", task.PartnerId.ToString() }
        };
        await NotiService.PushNotification.SendMessage(task.PartnerId.ToString(),
            $"Thông báo bình luận.",
            $"Bạn có 1 bình luận mới.", data1);
        
        Dictionary<string, string> data2 = new Dictionary<string, string>()
        {
            { "type", "Comment" },
            { "userId", task.StaffId.ToString() }
        };
        await NotiService.PushNotification.SendMessage(task.StaffId.ToString(),
            $"Thông báo bình luận.",
            $"Bạn có 1 bình luận mới.", data2);
        
        var query = await _commentRepo.CreateComment(comment);

        var result = this._mapper.Map<CommentResponse>(query);

        return result;
    }
}