namespace WSS.API.Data.Repositories.Customer;

public interface ICustomerRepo
{
    IQueryable<Models.Customer> GetCustomers(Expression<Func<Models.Customer, bool>>? predicate = null,
        Expression<Func<Models.Customer, object>>[]? includeProperties = null);

    Task<Models.Customer?> GetCustomerById(Guid id, Expression<Func<Models.Customer, object>>[]? includeProperties = null);
    Task<Models.Customer> CreateCustomer(Models.Customer user, bool tempSave = false);
    Task<Models.Customer> UpdateCustomer(Models.Customer user, bool tempSave = false);
    Task<Models.Customer> DeleteCustomer(Models.Customer user, bool tempSave = false);
}