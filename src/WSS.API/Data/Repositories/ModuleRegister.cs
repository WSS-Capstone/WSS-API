using WSS.API.Data.Repositories.Account;
using WSS.API.Data.Repositories.Cart;
using WSS.API.Data.Repositories.Category;
using WSS.API.Data.Repositories.Combo;
using WSS.API.Data.Repositories.ComboServices;
using WSS.API.Data.Repositories.Commission;
using WSS.API.Data.Repositories.CurrentPrice;
using WSS.API.Data.Repositories.Customer;
using WSS.API.Data.Repositories.Feedback;
using WSS.API.Data.Repositories.Message;
using WSS.API.Data.Repositories.Order;
using WSS.API.Data.Repositories.OrderDetail;
using WSS.API.Data.Repositories.Owner;
using WSS.API.Data.Repositories.Partner;
using WSS.API.Data.Repositories.PartnerPaymentHistory;
using WSS.API.Data.Repositories.PartnerService;
using WSS.API.Data.Repositories.PaymentHistory;
using WSS.API.Data.Repositories.Role;
using WSS.API.Data.Repositories.Service;
using WSS.API.Data.Repositories.ServiceImage;
using WSS.API.Data.Repositories.staff;
using WSS.API.Data.Repositories.Staff;
using WSS.API.Data.Repositories.Task;
using WSS.API.Data.Repositories.Voucher;
using WSS.API.Data.Repositories.WeddingInformation;

namespace WSS.API.Data.Repositories;

public static class ModuleRegister
{
    public static void RegisterDataRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAccountRepo, AccountRepo>();
        services.AddScoped<ICartRepo, CartRepo>();
        services.AddScoped<ICategoryRepo, CategoryRepo>();
        services.AddScoped<IComboRepo, ComboRepo>();
        services.AddScoped<IComboServiceRepo, ComboServiceRepo>();
        services.AddScoped<ICommissionRepo, CommissionRepo>();
        services.AddScoped<ICurrentPriceRepo, CurrentPriceRepo>();
        services.AddScoped<ICustomerRepo, CustomerRepo>();
        services.AddScoped<IFeedbackRepo, FeedbackRepo>();
        services.AddScoped<IMessageRepo, MessageRepo>();
        services.AddScoped<IOrderRepo, OrderRepo>();
        services.AddScoped<IOrderDetailRepo, OrderDetailRepo>();
        services.AddScoped<IOwnerRepo, OwnerRepo>();
        services.AddScoped<IPartnerRepo, PartnerRepo>();
        services.AddScoped<IPartnerPaymentHistoryRepo, PartnerPaymentHistoryRepo>();
        services.AddScoped<IPartnerServiceRepo, PartnerServiceRepo>();
        services.AddScoped<IPaymentHistoryRepo, PaymentHistoryRepo>();
        services.AddScoped<IRoleRepo, RoleRepo>();
        services.AddScoped<IServiceRepo, ServiceRepo>();
        services.AddScoped<IServiceImageRepo, ServiceImageRepo>();
        services.AddScoped<IStaffRepo, StaffRepo>();
        services.AddScoped<ITaskRepo, TaskRepo>();
        services.AddScoped<IVoucherRepo, VoucherRepo>();
        services.AddScoped<IWeddingInformationRepo, WeddingInformationRepo>();
    }
}