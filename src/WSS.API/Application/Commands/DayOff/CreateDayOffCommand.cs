using WSS.API.Application.Queries.DayOff;
using WSS.API.Data.Repositories.DayOff;
using WSS.API.Infrastructure.Utilities;

namespace WSS.API.Application.Commands.DayOff;

public class CreateDayOffCommand : IRequest<DayOffResponse>
{
    // public string? Code { get; set; }
    public Guid? PartnerId { get; set; }

    public DateTime? Day { get; set; }
    public string? Reason { get; set; }
}

public class CreateDayOffRequest
{
    public DateTime? Day { get; set; }
    public string? Reason { get; set; }
}

public class CreateDayOffCommandHandler : IRequestHandler<CreateDayOffCommand, DayOffResponse>
{
    private readonly IMapper _mapper;
    private readonly IDayOffRepo _dayOffRepo;

    public CreateDayOffCommandHandler(IMapper mapper, IDayOffRepo dayOffRepo)
    {
        _mapper = mapper;
        _dayOffRepo = dayOffRepo;
    }

    public async Task<DayOffResponse> Handle(CreateDayOffCommand request, CancellationToken cancellationToken)
    {
        var code = await _dayOffRepo.GetDayOffs().OrderByDescending(x => x.Code).Select(x => x.Code)
            .FirstOrDefaultAsync(cancellationToken);

        var exist = await _dayOffRepo.GetDayOffs(x => x.Day.Value.Date == request.Day.Value.Date
                                                      && x.PartnerId == request.PartnerId &&
                                                      x.Status == (int?)DayOffStatus.Active)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (exist != null)
        {
            throw new Exception("Ngày nghỉ đã tồn tại");
        }


        var dayOff = _mapper.Map<Data.Models.DayOff>(request);
        dayOff.Id = Guid.NewGuid();
        dayOff.Code = GenCode.NextId(code, "D");
        dayOff.Status = (int)DayOffStatus.Active;
        dayOff = await _dayOffRepo.CreateDayOff(dayOff);

        return _mapper.Map<DayOffResponse>(dayOff);
    }
}