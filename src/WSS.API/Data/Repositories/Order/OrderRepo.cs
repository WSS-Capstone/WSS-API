namespace WSS.API.Data.Repositories.Order;

public class OrderRepo : IOrderRepo
{
     private readonly IDbContextFactory _dbContextFactory;
    private readonly IGenericRepository<Models.Order> _repo;

    /// <summary>
    ///     Init
    /// </summary>
    /// <param name="dbContextFactory"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public OrderRepo(IDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
        _repo = _dbContextFactory.UnitOfWork<WSSContext, Models.Order>().Repository ??
                throw new InvalidOperationException();
    }

    /// <inheritdoc />
    public IQueryable<Models.Order> GetOrders(Expression<Func<Models.Order, bool>>? predicate = null,
        Expression<Func<Models.Order, object>>[]? includeProperties = null)
    {
        return _repo.Get(predicate, includeProperties);
    }

    /// <inheritdoc />
    public async Task<Models.Order> CreateOrder(Models.Order order, bool tempSave = false)
    {
        await _repo.InsertAsync(order);

        _ = tempSave
            ? await _dbContextFactory.UnitOfWork<WSSContext, Models.Order>().SaveTempChangesAsync()
            : await _dbContextFactory.SaveAllAsync();

        return order;
    }

    /// <inheritdoc />
    public async Task<Models.Order> UpdateOrder(Models.Order order, bool tempSave = false)
    {
        await _repo.UpdateAsync(order);

        _ = tempSave
            ? await _dbContextFactory.UnitOfWork<WSSContext, Models.Order>().SaveTempChangesAsync()
            : await _dbContextFactory.SaveAllAsync();

        return order;
    }

    /// <inheritdoc />
    public async Task<Models.Order> DeleteOrder(Models.Order order, bool tempSave = false)
    {
        await _repo.DeleteAsync(order);

        _ = tempSave
            ? await _dbContextFactory.UnitOfWork<WSSContext, Models.Order>().SaveTempChangesAsync()
            : await _dbContextFactory.SaveAllAsync();

        return order;
    }

    /// <inheritdoc />
    public async Task<Models.Order?> GetOrderById(Guid id,
        Expression<Func<Models.Order, object>>[]? includeProperties = null)
    {
        var order = await _repo.GetByIdAsync(id, includeProperties);
        return order;
    }
}