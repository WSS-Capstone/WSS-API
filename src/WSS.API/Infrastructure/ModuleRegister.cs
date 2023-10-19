using WSS.API.Infrastructure.Services.File;
using WSS.API.Infrastructure.Services.Identity;
using WSS.API.Infrastructure.Services.Mail;
using WSS.API.Infrastructure.Services.VnPay;

namespace WSS.API.Infrastructure;

/// <summary>
/// Add custom services into Dependency Injection Container.
/// </summary>
public static class ModuleRegister
{
    /// <summary>
    /// DI Service classes for Business.
    /// </summary>
    /// <param name="services">Service container from Startup.</param>
    public static void RegisterServiceModule(this IServiceCollection services)
    {
        services.AddTransient<IIdentitySvc, IdentitySvc>();
        services.AddScoped<IFileSvc, FileSvc>();
        services.AddScoped<VnPayService>();
        services.AddScoped<IMailService, MailService>();
    }
}
