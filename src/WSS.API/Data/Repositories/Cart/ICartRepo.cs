namespace WSS.API.Data.Repositories.Cart;

public interface ICartRepo
{
    IQueryable<Models.Cart> GetCarts(Expression<Func<Models.Cart, bool>>? predicate = null,
        Expression<Func<Models.Cart, object>>[]? includeProperties = null);

    Task<Models.Cart?> GetCartById(Guid id, Expression<Func<Models.Cart, object>>[]? includeProperties = null);
    Task<Models.Cart> CreateCart(Models.Cart user, bool tempSave = false);
    Task<Models.Cart> UpdateCart(Models.Cart user, bool tempSave = false);
    Task<Models.Cart> DeleteCart(Models.Cart user, bool tempSave = false);
}