using WSS.API.Application.Queries.DayOff;
using WSS.API.Data.Repositories.DayOff;

namespace WSS.API.Application.Commands.DayOff;

public class UpdateDayOffCommand : IRequest<DayOffResponse>
{
    public Guid Id { get; set; }
    public Guid? PartnerId { get; set; }
    public Guid? ServiceId { get; set; }
    public DateTime? Day { get; set; }
    public string? Reason { get; set; }
}

public class UpdateDayOffRequest
{
    public DateTime? Day { get; set; }
    public Guid? ServiceId { get; set; }
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
        var exist = await _repo.GetDayOffs(x => x.Day.Value.Date == request.Day.Value.Date
                                                && x.PartnerId == request.PartnerId 
                                                && x.Id != request.Id 
                                                && x.Status == (int?)DayOffStatus.Active)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (exist != null)
        {
            throw new Exception("Ngày nghỉ đã tồn tại");
        }
        
        var dayoff = await _repo.GetDayOffById(request.Id);
        if (dayoff == null)
        {
            throw new Exception("Day off not found");
        }
        
        dayoff = this._mapper.Map(request, dayoff);
        
        await _repo.UpdateDayOff(dayoff);
        var result = this._mapper.Map<DayOffResponse>(dayoff);

        return result;
    }
}