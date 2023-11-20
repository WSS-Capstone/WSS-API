using WSS.API.Data.Repositories.Feedback;

namespace WSS.API.Application.Feedback;

public class UpdateStatusFeedbackCommand : IRequest<FeedbackResponse>
{
    public Guid Id { get; set; }
    public FeedbackStatus Status { get; set; }
}

public class UpdateStatusFeedbackCommandHandler : IRequestHandler<UpdateStatusFeedbackCommand, FeedbackResponse>
{
    private readonly IFeedbackRepo _repo;
    private readonly IMapper _mapper;

    public UpdateStatusFeedbackCommandHandler(IFeedbackRepo repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<FeedbackResponse> Handle(UpdateStatusFeedbackCommand request, CancellationToken cancellationToken)
    {
        var feedback = await _repo.GetFeedbackById(request.Id);
        if (feedback == null)
        {
            throw new Exception("Feedback not found");
        }

        feedback.Status = (int)request.Status;
        feedback = await _repo.UpdateFeedback(feedback);
        return _mapper.Map<FeedbackResponse>(feedback);
    }
}