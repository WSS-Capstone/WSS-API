namespace WSS.API.Data.Repositories.Voucher;

/// <summary>
/// 
/// </summary>
public interface IVoucherRepo
{
    IQueryable<Models.Voucher> GetVouchers(Expression<Func<Models.Voucher, bool>>? predicate = null,
        Expression<Func<Models.Voucher, object>>[]? includeProperties = null);

    Task<Models.Voucher?> GetVoucherById(Guid id, Expression<Func<Models.Voucher, object>>[]? includeProperties = null);
    Task<Models.Voucher> CreateVoucher(Models.Voucher user, bool tempSave = false);
    Task<Models.Voucher> UpdateVoucher(Models.Voucher user, bool tempSave = false);
    Task<Models.Voucher> DeleteVoucher(Models.Voucher user, bool tempSave = false);
}