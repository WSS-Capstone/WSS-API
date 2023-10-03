using WSS.API.Data.Repositories.Commission;

namespace WSS.API.Application.Commands.Commission;

public class UpdateCommissionCommand : IRequest<CommissionResponse>
{
    public UpdateCommissionCommand(Guid id, CreateCommissionCommand command)
    {
        Id = id;
        CategoryId = command.CategoryId;
        DateOfApply = command.DateOfApply;
        CommisionValue = command.CommisionValue;
    }

    public Guid Id { get; set; }
    public Guid? CategoryId { get; set; }
    public DateTime? DateOfApply { get; set; }
    public double? CommisionValue { get; set; }
}

public class UpdateCommissionCommandHandler : IRequestHandler<UpdateCommissionCommand, CommissionResponse>
{
    private readonly IMapper _mapper;
    private readonly ICommissionRepo _commissionRepo;

    public UpdateCommissionCommandHandler(IMapper mapper, ICommissionRepo commissionRepo)
    {
        _mapper = mapper;
        _commissionRepo = commissionRepo;
    }

    public async Task<CommissionResponse> Handle(UpdateCommissionCommand request, CancellationToken cancellationToken)
    {
        var commission = await _commissionRepo.GetCommissionById(request.Id);
        if (commission == null)
        {
            throw new Exception(nameof(Commission) + request.Id);
        }

        commission = _mapper.Map(request, commission);
        commission = await _commissionRepo.UpdateCommission(commission);
        return _mapper.Map<CommissionResponse>(commission);
    }
}