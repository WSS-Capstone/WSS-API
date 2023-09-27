using L.Core.Logging.ConsoleLog;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace L.Core.Logging;

public static class ModuleRegister
{
    /// <summary>
    /// DI logging handlers.
    /// </summary>
    /// <param name="services">Service container from Program.</param>
    /// <param name="prefixFileLog">Prefix of file name.</param>
    public static void RegisterLogging(this IServiceCollection services, string prefixFileLog = "log")
    {
        services.AddLogging(logging  =>
        {
            logging.ClearProviders();
            logging.AddLConsole(configuration =>
            {
                // Replace warning value from appsettings.json of "Cyan"
                configuration.LogLevelToColorMap[LogLevel.Warning] = ConsoleColor.DarkCyan;
                // Replace warning value from appsettings.json of "Red"
                configuration.LogLevelToColorMap[LogLevel.Error] = ConsoleColor.DarkRed;
            });
            // logging.AddConsole();
            logging.AddDebug();
            logging.AddFile($"Log/{prefixFileLog}.txt");
        });

    }
}