using WSS.API.Data.Repositories.Commission;

namespace WSS.API.Application.Commands.Commission;

public class CreateCommissionCommand : IRequest<CommissionResponse>
{
    public Guid? CategoryId { get; set; }
    public DateTime? DateOfApply { get; set; }
    public double? CommisionValue { get; set; }
}

public class CreateCommissionCommandHandler : IRequestHandler<CreateCommissionCommand, CommissionResponse>
{
    private readonly IMapper _mapper;
    private readonly ICommissionRepo _commissionRepo;

    public CreateCommissionCommandHandler(IMapper mapper, ICommissionRepo commissionRepo)
    {
        _mapper = mapper;
        _commissionRepo = commissionRepo;
    }

    public async Task<CommissionResponse> Handle(CreateCommissionCommand request, CancellationToken cancellationToken)
    {
        var commission = _mapper.Map<Data.Models.Commission>(request);
        commission.Id = Guid.NewGuid();
        
        commission = await _commissionRepo.CreateCommission(commission);
        
        return _mapper.Map<CommissionResponse>(commission);
    }
}