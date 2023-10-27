using WSS.API.Application.Queries.DayOff;
using WSS.API.Data.Repositories.DayOff;

namespace WSS.API.Application.Commands.DayOff;

public class DeleteDayOffCommand : IRequest<DayOffResponse>
{
    public Guid Id { get; set; }
    public Guid? PartnerId { get; set; }
    public string? Reason { get; set; }
}

public class DeleteDayOffRequest
{
    public string? Reason { get; set; }
}

public class DeleteDayOffCommandHandler : IRequestHandler<DeleteDayOffCommand, DayOffResponse>
{
    private readonly IMapper _mapper;
    private readonly IDayOffRepo _repo;

    public DeleteDayOffCommandHandler(IMapper mapper, IDayOffRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<DayOffResponse> Handle(DeleteDayOffCommand request, CancellationToken cancellationToken)
    {
        var dayoff = await _repo.GetDayOffById(request.Id);
        if (dayoff == null)
        {
            throw new Exception("Day off not found");
        }

        if (dayoff.PartnerId != request.PartnerId)
        {
            throw new Exception("Day off not found for this user");
        }

        dayoff.Status = (int)DayOffStatus.InActive;
        dayoff.Reason = request.Reason;
        await _repo.UpdateDayOff(dayoff);
        var result = this._mapper.Map<DayOffResponse>(dayoff);

        return result;
    }
}