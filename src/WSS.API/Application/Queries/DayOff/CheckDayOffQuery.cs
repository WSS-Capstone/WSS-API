using L.Core.Data.EFCore.UoW;
using WSS.API.Data.Repositories.Account;
using WSS.API.Data.Repositories.DayOff;
using WSS.API.Data.Repositories.Service;
using WSS.API.Infrastructure.Services.Identity;

namespace WSS.API.Application.Queries.DayOff;

public class CheckDayOffQuery : IRequest<DayOffResponse>
{
    public Guid ServiceId { get; set; }
    public DateTime DayOff { get; set; }
}

public class CheckDayOffQueryHandler : IRequestHandler<CheckDayOffQuery, DayOffResponse>
{
    private readonly IMapper _mapper;
    private readonly IAccountRepo _accountRepo;
    private readonly IIdentitySvc _identitySvc;
    private readonly IServiceRepo _serviceRepo;
    private readonly IDayOffRepo _dayOffRepo;

    public CheckDayOffQueryHandler(IAccountRepo accountRepo, IIdentitySvc identitySvc, IServiceRepo serviceRepo, IDayOffRepo dayOffRepo, IMapper mapper)
    {
        _accountRepo = accountRepo;
        _identitySvc = identitySvc;
        _serviceRepo = serviceRepo;
        _dayOffRepo = dayOffRepo;
        _mapper = mapper;
    }

    public async Task<DayOffResponse> Handle(CheckDayOffQuery request, CancellationToken cancellationToken)
    {
        var dayOff = _dayOffRepo.GetDayOffs(d => d.ServiceId == request.ServiceId && d.Day.Value.Date == request.DayOff.Date).FirstOrDefault();
        
        var result = this._mapper.Map<DayOffResponse>(dayOff);
        
        return result;
        
    }
}