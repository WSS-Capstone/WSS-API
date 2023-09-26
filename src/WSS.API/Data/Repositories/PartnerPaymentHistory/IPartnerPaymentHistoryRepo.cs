namespace WSS.API.Data.Repositories.PartnerPaymentHistory;

public interface IPartnerPaymentHistoryRepo
{
    IQueryable<Models.PartnerPaymentHistory> GetPartnerPaymentHistorys(Expression<Func<Models.PartnerPaymentHistory, bool>>? predicate = null,
        Expression<Func<Models.PartnerPaymentHistory, object>>[]? includeProperties = null);

    Task<Models.PartnerPaymentHistory?> GetPartnerPaymentHistoryById(Guid id, Expression<Func<Models.PartnerPaymentHistory, object>>[]? includeProperties = null);
    Task<Models.PartnerPaymentHistory> CreatePartnerPaymentHistory(Models.PartnerPaymentHistory user, bool tempSave = false);
    Task<Models.PartnerPaymentHistory> UpdatePartnerPaymentHistory(Models.PartnerPaymentHistory user, bool tempSave = false);
    Task<Models.PartnerPaymentHistory> DeletePartnerPaymentHistory(Models.PartnerPaymentHistory user, bool tempSave = false);
}