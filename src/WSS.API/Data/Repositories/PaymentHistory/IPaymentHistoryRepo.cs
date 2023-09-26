namespace WSS.API.Data.Repositories.PaymentHistory;

public interface IPaymentHistoryRepo
{
    IQueryable<Models.PaymentHistory> GetPaymentHistorys(Expression<Func<Models.PaymentHistory, bool>>? predicate = null,
        Expression<Func<Models.PaymentHistory, object>>[]? includeProperties = null);

    Task<Models.PaymentHistory?> GetPaymentHistoryById(Guid id, Expression<Func<Models.PaymentHistory, object>>[]? includeProperties = null);
    Task<Models.PaymentHistory> CreatePaymentHistory(Models.PaymentHistory user, bool tempSave = false);
    Task<Models.PaymentHistory> UpdatePaymentHistory(Models.PaymentHistory user, bool tempSave = false);
    Task<Models.PaymentHistory> DeletePaymentHistory(Models.PaymentHistory user, bool tempSave = false);
}