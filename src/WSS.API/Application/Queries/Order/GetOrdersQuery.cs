namespace WSS.API.Application.Queries.Order;

public class GetOrdersQuery : PagingParam<OrderSortCriteria>, IRequest<PagingResponseQuery<OrderResponse, OrderSortCriteria>>
{
    
}

public enum OrderSortCriteria
{
    Id,
    Fullname,
    Address,
    Phone
}