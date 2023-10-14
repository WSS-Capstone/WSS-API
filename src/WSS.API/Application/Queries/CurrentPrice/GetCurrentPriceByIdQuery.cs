namespace WSS.API.Application.Queries.CurrentPrice;

public class GetCurrentPriceByIdQuery : IRequest<CurrentPriceResponse>
{
    public GetCurrentPriceByIdQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}