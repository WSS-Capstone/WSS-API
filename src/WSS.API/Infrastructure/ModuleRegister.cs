using WSS.API.Infrastructure.Services.Identity;

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
    }
}
