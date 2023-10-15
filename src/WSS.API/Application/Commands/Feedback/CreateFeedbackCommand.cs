using WSS.API.Data.Repositories.Feedback;

namespace WSS.API.Application.Feedback;

public class CreateFeedbackCommand : IRequest<FeedbackResponse>
{
    public string? Content { get; set; }
    public int? Rating { get; set; }
    public Guid? OrderDetailId { get; set; }
    public Guid? UserId { get; set; }
    public int? Status { get; set; }
}

public class CreateFeedbackCommandHandler : IRequestHandler<CreateFeedbackCommand, FeedbackResponse>
{
    private readonly IMapper _mapper;
    private readonly IFeedbackRepo _feedbackRepo;

    public CreateFeedbackCommandHandler(IMapper mapper, IFeedbackRepo feedbackRepo)
    {
        _mapper = mapper;
        _feedbackRepo = feedbackRepo;
    }

    public async Task<FeedbackResponse> Handle(CreateFeedbackCommand request, CancellationToken cancellationToken)
    {
        var feedback = _mapper.Map<Data.Models.Feedback>(request);
        feedback.Id = Guid.NewGuid();
        feedback.CreateDate = DateTime.UtcNow;
        
        feedback = await _feedbackRepo.CreateFeedback(feedback);
        
        return _mapper.Map<FeedbackResponse>(feedback);
    }
}