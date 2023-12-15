using WSS.API.Data.Repositories.Combo;
using WSS.API.Data.Repositories.Service;
using WSS.API.Infrastructure.Utilities;

namespace WSS.API.Application.Commands.Combo;

public class AddNewComboCommand : IRequest<ComboResponse>
{
    public string? Name { get; set; }
    public double? DiscountValueCombo { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public Guid[] ComboServicesId { get; set; }
}

public class AddNewComboCommandHandler : IRequestHandler<AddNewComboCommand, ComboResponse>
{
    private readonly IComboRepo _comboRepo;
    private readonly IServiceRepo _serviceRepo;
    private readonly IMapper _mapper;

    public AddNewComboCommandHandler(IComboRepo comboRepo, IMapper mapper, IServiceRepo serviceRepo)
    {
        _comboRepo = comboRepo;
        _mapper = mapper;
        _serviceRepo = serviceRepo;
    }

    public async Task<ComboResponse> Handle(AddNewComboCommand request, CancellationToken cancellationToken)
    {
        var code = await _comboRepo.GetCombos().OrderByDescending(x => x.Code).Select(x => x.Code)
            .FirstOrDefaultAsync(cancellationToken);
        var combo = _mapper.Map<Data.Models.Combo>(request);
        var newId = Guid.NewGuid();
        combo.Id = newId;
        combo.Code = GenCode.NextId(code, "CB");
        combo.Status = (int?)CategoryStatus.Active;
        
        var serviceInCombo = await this._serviceRepo.GetServices(s => request.ComboServicesId.Contains(s.Id), new Expression<Func<Data.Models.Service, object>>[]
            {
                s => s.CurrentPrices
            }).ToListAsync(cancellationToken: cancellationToken);
        
        var serviceWprice = this._mapper.Map<List<ServiceResponse>>(serviceInCombo);
        var serviceCombos = serviceWprice.Select(s => new ComboService()
        {
            ServiceId = s.Id,
            ComboId = newId,
            Id = Guid.NewGuid(),
            CreateDate = DateTime.Now
        }).ToList();
        
        var totalAmount = serviceWprice.Sum(s => s.CurrentPrices?.Price ?? 0);
        
        combo.ComboServices = serviceWprice.Count != 0 ? serviceCombos : null;
        combo.TotalAmount = totalAmount;
        
        var query = await _comboRepo.CreateCombo(combo);
        
        var result = this._mapper.Map<ComboResponse>(query);
        
        return result;
    }
}