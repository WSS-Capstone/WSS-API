using WSS.API.Data.Repositories.Combo;

namespace WSS.API.Application.Commands.Combo;

public class DeleteComboCommand : IRequest<ComboResponse>
{
    public DeleteComboCommand(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}

public class DeleteComboCommandHandler : IRequestHandler<DeleteComboCommand, ComboResponse>
{
    private readonly IComboRepo _comboRepo;
    private readonly IMapper _mapper;

    public DeleteComboCommandHandler(IComboRepo comboRepo, IMapper mapper)
    {
        _comboRepo = comboRepo;
        _mapper = mapper;
    }

    public async Task<ComboResponse> Handle(DeleteComboCommand request, CancellationToken cancellationToken)
    {
        var combo = await this._comboRepo.GetComboById(request.Id, new Expression<Func<Data.Models.Combo, object>>[]
        {
            c => c.ComboServices
        });
        
        if (combo == null)
        {
            throw new Exception("Combo not found");
        }
        
        // combo.Status = (int?)CategoryStatus.InActive;
        var query = await _comboRepo.DeleteCombo(combo);
        
        var result = this._mapper.Map<ComboResponse>(query);
        
        return result;
    }
}