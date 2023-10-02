using WSS.API.Data.Repositories.Combo;

namespace WSS.API.Application.Commands.Combo;

public class UpdateComboCommand : IRequest<ComboResponse>
{
    public UpdateComboCommand(Guid id, AddNewComboCommand command)
    {
        Id = id;
        Name = command.Name;
        DiscountValueCombo = command.DiscountValueCombo;
        TotalAmount = command.TotalAmount;
        Description = command.Description;
        ComboServices = command.ComboServices;
    }

    public Guid Id { get; set; }
    public string? Name { get; set; }
    public double? DiscountValueCombo { get; set; }
    public double? TotalAmount { get; set; }
    public string? Description { get; set; }
    public List<ComboServicesResponse> ComboServices { get; set; }
}

public class UpdateComboCommandHandler : IRequestHandler<UpdateComboCommand, ComboResponse>
{
    private readonly IComboRepo _comboRepo;
    private readonly IMapper _mapper;

    public UpdateComboCommandHandler(IComboRepo comboRepo, IMapper mapper)
    {
        _comboRepo = comboRepo;
        _mapper = mapper;
    }

    public async Task<ComboResponse> Handle(UpdateComboCommand request, CancellationToken cancellationToken)
    {
        var comboInDb = await _comboRepo.GetComboById(request.Id, new Expression<Func<Data.Models.Combo, object>>[]
        {
            c => c.ComboServices
        });
        
        if (comboInDb == null)
        {
            throw new Exception("Combo not found");
        }
        
        var combo = _mapper.Map(request, comboInDb);
        var query = await _comboRepo.UpdateCombo(combo);
        
        var result = this._mapper.Map<ComboResponse>(query);
        
        return result;
    }
}