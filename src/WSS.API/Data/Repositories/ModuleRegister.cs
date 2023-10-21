using WSS.API.Data.Repositories.Account;
using WSS.API.Data.Repositories.Category;
using WSS.API.Data.Repositories.Combo;
using WSS.API.Data.Repositories.ComboServices;
using WSS.API.Data.Repositories.Commission;
using WSS.API.Data.Repositories.CurrentPrice;
using WSS.API.Data.Repositories.DayOff;
using WSS.API.Data.Repositories.Feedback;
using WSS.API.Data.Repositories.Message;
using WSS.API.Data.Repositories.Order;
using WSS.API.Data.Repositories.OrderDetail;
using WSS.API.Data.Repositories.PartnerPaymentHistory;
using WSS.API.Data.Repositories.PaymentHistory;
using WSS.API.Data.Repositories.Service;
using WSS.API.Data.Repositories.ServiceImage;
using WSS.API.Data.Repositories.Task;
using WSS.API.Data.Repositories.User;
using WSS.API.Data.Repositories.Voucher;
using WSS.API.Data.Repositories.WeddingInformation;

namespace WSS.API.Data.Repositories;

/// <summary>
/// 
/// </summary>
public static class ModuleRegister
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    public static void RegisterDataRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAccountRepo, AccountRepo>();
        services.AddScoped<ICategoryRepo, CategoryRepo>();
        services.AddScoped<IComboRepo, ComboRepo>();
        services.AddScoped<IComboServiceRepo, ComboServiceRepo>();
        services.AddScoped<ICommissionRepo, CommissionRepo>();
        services.AddScoped<ICurrentPriceRepo, CurrentPriceRepo>();
        services.AddScoped<IFeedbackRepo, FeedbackRepo>();
        services.AddScoped<IMessageRepo, MessageRepo>();
        services.AddScoped<IOrderRepo, OrderRepo>();
        services.AddScoped<IOrderDetailRepo, OrderDetailRepo>();
        services.AddScoped<IPartnerPaymentHistoryRepo, PartnerPaymentHistoryRepo>();
        services.AddScoped<IPaymentHistoryRepo, PaymentHistoryRepo>();
        services.AddScoped<IServiceRepo, ServiceRepo>();
        services.AddScoped<IServiceImageRepo, ServiceImageRepo>();
        services.AddScoped<ITaskRepo, TaskRepo>();
        services.AddScoped<IVoucherRepo, VoucherRepo>();
        services.AddScoped<IWeddingInformationRepo, WeddingInformationRepo>();
        services.AddScoped<IUserRepo, UserRepo>();
        services.AddScoped<IDayOffRepo, DayOffRepo>();
    }
}