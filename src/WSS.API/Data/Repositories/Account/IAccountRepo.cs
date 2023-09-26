namespace WSS.API.Data.Repositories.Account;

public interface IAccountRepo
{
    IQueryable<Models.Account> GetAccounts(Expression<Func<Models.Account, bool>>? predicate = null,
        Expression<Func<Models.Account, object>>[]? includeProperties = null);

    Task<Models.Account?> GetAccountById(Guid id, Expression<Func<Models.Account, object>>[]? includeProperties = null);
    Task<Models.Account> CreateAccount(Models.Account user, bool tempSave = false);
    Task<Models.Account> UpdateAccount(Models.Account user, bool tempSave = false);
    Task<Models.Account> DeleteAccount(Models.Account user, bool tempSave = false);
}