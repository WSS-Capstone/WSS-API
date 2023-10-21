using WSS.API.Data.Repositories.Combo;
using WSS.API.Infrastructure.Utilities;

namespace WSS.API.Application.Commands.Combo;

public class AddNewComboCommand : IRequest<ComboService>
{
    public string? Name { get; set; }
    public double? DiscountValueCombo { get; set; }
    public double? TotalAmount { get; set; }
    public string? Description { get; set; }
    public List<ComboServicesResponse> ComboServices { get; set; }
}

public class AddNewComboCommandHandler : IRequestHandler<AddNewComboCommand, ComboService>
{
    private readonly IComboRepo _comboRepo;
    private readonly IMapper _mapper;

    public AddNewComboCommandHandler(IComboRepo comboRepo, IMapper mapper)
    {
        _comboRepo = comboRepo;
        _mapper = mapper;
    }

    public async Task<ComboService> Handle(AddNewComboCommand request, CancellationToken cancellationToken)
    {
        var code = await _comboRepo.GetCombos().OrderByDescending(x => x.Code).Select(x => x.Code)
            .FirstOrDefaultAsync(cancellationToken);
        var combo = _mapper.Map<Data.Models.Combo>(request);
        combo.Id = Guid.NewGuid();
        combo.Code = GenCode.NextId(code);
        combo.Status = (int?)CategoryStatus.Active;
        var query = await _comboRepo.CreateCombo(combo);
        
        var result = this._mapper.Map<ComboService>(query);
        
        return result;
    }
}