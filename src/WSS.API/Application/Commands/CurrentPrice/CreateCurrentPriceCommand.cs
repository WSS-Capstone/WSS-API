using WSS.API.Data.Repositories.CurrentPrice;

namespace WSS.API.Application.Commands.CurrentPrice;

public class CreateCurrentPriceCommand : IRequest<CurrentPriceResponse>
{
    public DateTime? DateOfApply { get; set; }
    public Guid? ServiceId { get; set; }
    public double? Price { get; set; }
}

public class CreateCurrentPriceCommandHandler : IRequestHandler<CreateCurrentPriceCommand, CurrentPriceResponse>
{
    private IMapper _mapper;
    private ICurrentPriceRepo _currentPriceRepo;

    public CreateCurrentPriceCommandHandler(IMapper mapper, ICurrentPriceRepo currentPriceRepo)
    {
        _mapper = mapper;
        _currentPriceRepo = currentPriceRepo;
    }

    /// <summary>
    /// Handle create currentPrice command
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<CurrentPriceResponse> Handle(CreateCurrentPriceCommand request, CancellationToken cancellationToken)
    {
        var currentPrice = _mapper.Map<Data.Models.CurrentPrice>(request);
        currentPrice.Id = Guid.NewGuid();
        currentPrice.CreateDate = DateTime.Now;
        var query = await _currentPriceRepo.CreateCurrentPrice(currentPrice);
        query = await this._currentPriceRepo.GetCurrentPriceById(query.Id, new Expression<Func<Data.Models.CurrentPrice, object>>[]
        {
            s => s.Service
        });

        var result = this._mapper.Map<CurrentPriceResponse>(query);
        
        return result;
    }
}