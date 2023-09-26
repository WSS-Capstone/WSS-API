namespace WSS.API.Data.Repositories.Order;

public interface IOrderRepo
{
    IQueryable<Models.Order> GetOrders(Expression<Func<Models.Order, bool>>? predicate = null,
        Expression<Func<Models.Order, object>>[]? includeProperties = null);

    Task<Models.Order?> GetOrderById(Guid id, Expression<Func<Models.Order, object>>[]? includeProperties = null);
    Task<Models.Order> CreateOrder(Models.Order user, bool tempSave = false);
    Task<Models.Order> UpdateOrder(Models.Order user, bool tempSave = false);
    Task<Models.Order> DeleteOrder(Models.Order user, bool tempSave = false);
}