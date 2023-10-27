using WSS.API.Data.Repositories.Combo;
using WSS.API.Data.Repositories.Service;

namespace WSS.API.Application.Commands.Combo;

public class UpdateComboCommand : IRequest<ComboResponse>
{
    public UpdateComboCommand(Guid id, AddNewComboCommand command)
    {
        Id = id;
        Name = command.Name;
        DiscountValueCombo = command.DiscountValueCombo;
        Description = command.Description;
        ComboServicesId = command.ComboServicesId;
    }

    public Guid Id { get; set; }
    public string? Name { get; set; }
    public double? DiscountValueCombo { get; set; }
    public string? Description { get; set; }
    public Guid[] ComboServicesId { get; set; }
}

public class UpdateComboCommandHandler : IRequestHandler<UpdateComboCommand, ComboResponse>
{
    private readonly IComboRepo _comboRepo;
    private readonly IMapper _mapper;
    private readonly IServiceRepo _serviceRepo;

    public UpdateComboCommandHandler(IComboRepo comboRepo, IMapper mapper, IServiceRepo serviceRepo)
    {
        _comboRepo = comboRepo;
        _mapper = mapper;
        _serviceRepo = serviceRepo;
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
        
        var serviceInCombo = await this._serviceRepo.GetServices(s => request.ComboServicesId.Contains(s.Id), new Expression<Func<Data.Models.Service, object>>[]
        {
            s => s.CurrentPrices
        }).ToListAsync(cancellationToken: cancellationToken);
        serviceInCombo.ForEach(s => s.Category?.Services.Clear());
        var serviceWprice = this._mapper.ProjectTo<ServiceResponse>(serviceInCombo.AsQueryable()).ToList();
        var serviceCombos = serviceWprice.Select(s => new ComboService()
        {
            ServiceId = s.Id,
            ComboId = comboInDb.Id,
            Id = Guid.NewGuid(),
            CreateDate = DateTime.Now
        }).ToList();
        
        var totalAmount = serviceWprice.Sum(s => s.CurrentPrices?.Price ?? 0);
        
        combo.ComboServices = serviceWprice.Count != 0 ? serviceCombos : null;
        combo.TotalAmount = totalAmount;

        var query = await _comboRepo.UpdateCombo(combo);
        
        var result = this._mapper.Map<ComboResponse>(query);
        
        return result;
    }
}