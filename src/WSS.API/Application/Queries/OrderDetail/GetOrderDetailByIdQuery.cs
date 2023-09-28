namespace WSS.API.Application.Queries.OrderDetail;

public class GetOrderDetailByIdQuery : IRequest<IList<OrderDetailResponse>>
{
    public GetOrderDetailByIdQuery(Guid orderId)
    {
        OrderId = orderId;
    }

    public Guid OrderId { get; set; }
}