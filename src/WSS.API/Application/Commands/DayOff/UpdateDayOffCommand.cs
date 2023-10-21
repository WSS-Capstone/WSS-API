using WSS.API.Data.Repositories.DayOff;

namespace WSS.API.Application.Commands.DayOff;

public class UpdateDayOffCommand : IRequest<DayOffResponse>
{
    public UpdateDayOffCommand(Guid id, UpdateDayOffRequest request)
    {
        Id = id;
        Code = request.Code;
        PartnerId = request.PartnerId;
        DayOff1 = request.DayOff1;
        Reason = request.Reason;
    }
    
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public Guid? PartnerId { get; set; }
    public Guid? DayOff1 { get; set; }
    public string? Reason { get; set; }
}

public class UpdateDayOffRequest
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public Guid? PartnerId { get; set; }
    public Guid? DayOff1 { get; set; }
    public string? Reason { get; set; }
}

public class UpdateDayOffCommandHandler : IRequestHandler<UpdateDayOffCommand, DayOffResponse>
{
    private IMapper _mapper;
    private IDayOffRepo _repo;

    public UpdateDayOffCommandHandler(IMapper mapper, IDayOffRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<DayOffResponse> Handle(UpdateDayOffCommand request, CancellationToken cancellationToken)
    {
        var feedback = await _repo.GetDayOffById(request.Id);
        if (feedback == null)
        {
            throw new Exception("Day off not found");
        }
       
        feedback = this._mapper.Map(request, feedback);
        
        await _repo.UpdateDayOff(feedback);
        var result = this._mapper.Map<DayOffResponse>(feedback);

        return result;
    }
}