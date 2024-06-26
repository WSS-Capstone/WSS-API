namespace WSS.API.Data.Repositories.OrderDetail;

public interface IOrderDetailRepo
{
    IQueryable<Models.OrderDetail> GetOrderDetails(Expression<Func<Models.OrderDetail, bool>>? predicate = null,
        Expression<Func<Models.OrderDetail, object>>[]? includeProperties = null);

    Task<Models.OrderDetail?> GetOrderDetailById(Guid id, Expression<Func<Models.OrderDetail, object>>[]? includeProperties = null);
    Task<Models.OrderDetail> CreateOrderDetail(Models.OrderDetail order, bool tempSave = false);
    Task<List<Models.OrderDetail>> CreateOrderDetails(List<Models.OrderDetail> orders, bool tempSave = false);
    Task<Models.OrderDetail> UpdateOrderDetail(Models.OrderDetail order, bool tempSave = false);
    Task<Models.OrderDetail> DeleteOrderDetail(Models.OrderDetail order, bool tempSave = false);
}