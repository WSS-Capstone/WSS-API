using WSS.API.Data.Repositories.OrderDetail;

namespace WSS.API.Application.Queries.OrderDetail;

public class GetOrderDetailByIdQuery : IRequest<IList<OrderDetailResponse>>
{
    public GetOrderDetailByIdQuery(Guid orderId)
    {
        OrderId = orderId;
    }

    public Guid OrderId { get; set; }
}

public class GetOrderDetailByIdQueryHandler : IRequestHandler<GetOrderDetailByIdQuery, IList<OrderDetailResponse>>
{
    private IMapper _mapper;
    private IOrderDetailRepo _repo;

    public GetOrderDetailByIdQueryHandler(IMapper mapper, IOrderDetailRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    /// <inheritdoc />
    public async Task<IList<OrderDetailResponse>> Handle(GetOrderDetailByIdQuery request,
        CancellationToken cancellationToken)
    {
        var query = _repo.GetOrderDetails(od => od.OrderId == request.OrderId,
            new Expression<Func<Data.Models.OrderDetail, object>>[]
            {
                o => o.Feedbacks,
                o => o.Order,
                o => o.Tasks,
                o => o.Service
            });

        var result = this._mapper.ProjectTo<OrderDetailResponse>(query);

        return await result.ToArrayAsync(cancellationToken: cancellationToken);
    }
}