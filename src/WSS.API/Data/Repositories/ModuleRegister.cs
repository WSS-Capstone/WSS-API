using WSS.API.Data.Repositories.Account;

namespace WSS.API.Data.Repositories;

public static class ModuleRegister
{
    public static void RegisterDataRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAccountRepo, AccountRepo>();
    }
}