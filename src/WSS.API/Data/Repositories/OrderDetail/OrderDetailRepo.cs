namespace WSS.API.Data.Repositories.OrderDetail;

public class OrderDetailRepo : IOrderDetailRepo
{
    private readonly IDbContextFactory _dbContextFactory;
    private readonly IGenericRepository<Models.OrderDetail> _repo;

    /// <summary>
    ///     Init
    /// </summary>
    /// <param name="dbContextFactory"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public OrderDetailRepo(IDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
        _repo = _dbContextFactory.UnitOfWork<WSSContext, Models.OrderDetail>().Repository ??
                throw new InvalidOperationException();
    }

    /// <inheritdoc />
    public IQueryable<Models.OrderDetail> GetOrderDetails(Expression<Func<Models.OrderDetail, bool>>? predicate = null,
        Expression<Func<Models.OrderDetail, object>>[]? includeProperties = null)
    {
        return _repo.Get(predicate, includeProperties);
    }

    /// <inheritdoc />
    public async Task<Models.OrderDetail> CreateOrderDetail(Models.OrderDetail order, bool tempSave = false)
    {
        await _repo.InsertAsync(order);

        _ = tempSave
            ? await _dbContextFactory.UnitOfWork<WSSContext, Models.OrderDetail>().SaveTempChangesAsync()
            : await _dbContextFactory.SaveAllAsync();

        return order;
    }
    /// <inheritdoc />
    public async Task<List<Models.OrderDetail>> CreateOrderDetails(List<Models.OrderDetail> orderDetails, bool tempSave = false)
    {
        await _repo.InsertAsync(orderDetails);

        _ = tempSave
            ? await _dbContextFactory.UnitOfWork<WSSContext, Models.OrderDetail>().SaveTempChangesAsync()
            : await _dbContextFactory.SaveAllAsync();

        return orderDetails;
    }

    /// <inheritdoc />
    public async Task<Models.OrderDetail> UpdateOrderDetail(Models.OrderDetail order, bool tempSave = false)
    {
        await _repo.UpdateAsync(order);

        _ = tempSave
            ? await _dbContextFactory.UnitOfWork<WSSContext, Models.OrderDetail>().SaveTempChangesAsync()
            : await _dbContextFactory.SaveAllAsync();

        return order;
    }

    /// <inheritdoc />
    public async Task<Models.OrderDetail> DeleteOrderDetail(Models.OrderDetail order, bool tempSave = false)
    {
        await _repo.DeleteAsync(order);

        _ = tempSave
            ? await _dbContextFactory.UnitOfWork<WSSContext, Models.OrderDetail>().SaveTempChangesAsync()
            : await _dbContextFactory.SaveAllAsync();

        return order;
    }

    /// <inheritdoc />
    public async Task<Models.OrderDetail?> GetOrderDetailById(Guid id,
        Expression<Func<Models.OrderDetail, object>>[]? includeProperties = null)
    {
        var order = await _repo.GetByIdAsync(id, includeProperties);
        return order;
    }
}