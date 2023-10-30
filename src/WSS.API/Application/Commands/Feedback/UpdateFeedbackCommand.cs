using WSS.API.Data.Repositories.Feedback;

namespace WSS.API.Application.Feedback;

public class UpdateFeedbackCommand : IRequest<FeedbackResponse>
{
    
    public Guid Id { get; set; }
    public string? Content { get; set; }
    public int? Rating { get; set; }
    public Guid? OrderDetailId { get; set; }
    public Guid? UserId { get; set; }
    public int? Status { get; set; }
}

public class UpdateFeedbackRequest
{
    public Guid Id { get; set; }
    public string? Content { get; set; }
    public int? Rating { get; set; }
    public Guid? OrderDetailId { get; set; }
}

public class UpdateFeedbackCommandHandler : IRequestHandler<UpdateFeedbackCommand, FeedbackResponse>
{
    private IMapper _mapper;
    private IFeedbackRepo _repo;

    public UpdateFeedbackCommandHandler(IMapper mapper, IFeedbackRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<FeedbackResponse> Handle(UpdateFeedbackCommand request, CancellationToken cancellationToken)
    {
        var feedback = await _repo.GetFeedbackById(request.Id);
        if (feedback == null)
        {
            throw new Exception("Feedback not found");
        }
       
        feedback = this._mapper.Map(request, feedback);
        
        await _repo.UpdateFeedback(feedback);
        var result = this._mapper.Map<FeedbackResponse>(feedback);

        return result;
    }
}