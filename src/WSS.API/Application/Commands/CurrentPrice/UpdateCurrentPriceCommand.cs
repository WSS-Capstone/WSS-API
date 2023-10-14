using WSS.API.Data.Repositories.CurrentPrice;

namespace WSS.API.Application.Commands.CurrentPrice;

public class UpdateCurrentPriceCommand : IRequest<CurrentPriceResponse>
{
    public UpdateCurrentPriceCommand(Guid id , UpdateCurrentPriceCommand command)
    {
        Id = id;
        DateOfApply = command.DateOfApply;
        ServiceId = command.ServiceId;
        Price = command.Price;
    }

    public Guid Id { get; set; }
    public DateTime? DateOfApply { get; set; }
    public Guid? ServiceId { get; set; }
    public double? Price { get; set; }
}

public class UpdateCurrentPriceCommandHandler : IRequestHandler<UpdateCurrentPriceCommand, CurrentPriceResponse>
{
    private IMapper _mapper;
    private ICurrentPriceRepo _currentPriceRepo;

    public UpdateCurrentPriceCommandHandler(IMapper mapper, ICurrentPriceRepo currentPriceRepo)
    {
        _mapper = mapper;
        _currentPriceRepo = currentPriceRepo;
    }

    /// <summary>
    /// Handle update current price command
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<CurrentPriceResponse> Handle(UpdateCurrentPriceCommand request, CancellationToken cancellationToken)
    {
        var currentPrice = await this._currentPriceRepo.GetCurrentPriceById(request.Id, new Expression<Func<Data.Models.CurrentPrice, object>>[]
        {
            s => s.Service
        });
        
        if (currentPrice == null)
        {
            throw new Exception("Current price not found");
        }
        
        currentPrice = _mapper.Map(request, currentPrice);
        currentPrice.UpdateDate = DateTime.Now;
        var query = await _currentPriceRepo.UpdateCurrentPrice(currentPrice);
        
        var result = this._mapper.Map<CurrentPriceResponse>(query);
        
        return result;
    }
}