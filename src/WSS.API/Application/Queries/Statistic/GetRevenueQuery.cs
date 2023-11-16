using WSS.API.Data.Repositories.Order;

namespace WSS.API.Application.Queries.Statistic;

public class GetRevenueQuery : IRequest<RevenueProfixRespponse>
{
    public DateTime FromDate { get; set; }
    public TypeStatistic Type { get; set; } = TypeStatistic.Day;
}

public enum TypeStatistic
{
    Day = 1,
    Month = 2,
    Year = 3
}


public class GetRevenueQueryHandler: IRequestHandler<GetRevenueQuery, RevenueProfixRespponse>
{
    private readonly IOrderRepo _repo;

    public GetRevenueQueryHandler(IOrderRepo repo)
    {
        _repo = repo;
    }

    public async Task<RevenueProfixRespponse> Handle(GetRevenueQuery request, CancellationToken cancellationToken)
    {
        var result = _repo.GetOrders();
        // switch (request.Type)
        // {
        //     case TypeStatistic.Day:
        //         result = result.Where(x => x.CreatedDate.Value.Date == DateTime.Now.Date);
        //         break;
        //     case TypeStatistic.Month:
        //         result = result.Where(x => x.CreatedDate.Value.Month == DateTime.Now.Month);
        //         break;
        //     case TypeStatistic.Year:
        //         result = result.Where(x => x.CreatedDate.Value.Year == DateTime.Now.Year);
        //         break;
        // }
        
        
        return null;
    }
}