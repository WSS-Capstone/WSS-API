using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace L.Core.Logging.ConsoleLog;

public static class ModuleRegister
{
    public static ILoggingBuilder AddLConsole(this ILoggingBuilder builder)
    {
        builder.AddConfiguration();
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, LConsoleLoggerProvider>());

        LoggerProviderOptions.RegisterProviderOptions<LConsoleLoggerConfig, LConsoleLoggerProvider>(builder.Services);

        return builder;
    }

    public static ILoggingBuilder AddLConsole(this ILoggingBuilder builder, Action<LConsoleLoggerConfig> configure)
    {
        builder.AddLConsole();
        builder.Services.Configure(configure);

        return builder;
    }
}