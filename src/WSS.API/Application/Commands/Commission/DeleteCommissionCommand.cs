using WSS.API.Data.Repositories.Commission;

namespace WSS.API.Application.Commands.Commission;

public class DeleteCommissionCommand : IRequest<CommissionResponse>
{
    public DeleteCommissionCommand(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}

public class DeleteCommissionCommandHandler : IRequestHandler<DeleteCommissionCommand, CommissionResponse>
{
    private readonly IMapper _mapper;
    private readonly ICommissionRepo _commissionRepo;

    public DeleteCommissionCommandHandler(IMapper mapper, ICommissionRepo commissionRepo)
    {
        _mapper = mapper;
        _commissionRepo = commissionRepo;
    }

    public async Task<CommissionResponse> Handle(DeleteCommissionCommand request, CancellationToken cancellationToken)
    {
        var commission = await _commissionRepo.GetCommissionById(request.Id);
        if (commission == null)
        {
            throw new Exception(nameof(Commission) + request.Id);
        }

        commission = await _commissionRepo.DeleteCommission(commission);
        return _mapper.Map<CommissionResponse>(commission);
    }
}