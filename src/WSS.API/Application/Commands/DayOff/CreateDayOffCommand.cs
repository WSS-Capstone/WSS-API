using WSS.API.Data.Repositories.DayOff;

namespace WSS.API.Application.Commands.DayOff;

public class CreateDayOffCommand : IRequest<DayOffResponse>
{
    public string? Code { get; set; }
    public Guid? PartnerId { get; set; }
    public Guid? DayOff1 { get; set; }
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
        var dayOff = _mapper.Map<Data.Models.DayOff>(request);
        dayOff.Id = Guid.NewGuid();
        
        dayOff = await _dayOffRepo.CreateDayOff(dayOff);
        
        return _mapper.Map<DayOffResponse>(dayOff);
    }
}