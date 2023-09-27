using System.Collections.Concurrent;
using System.Runtime.Versioning;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace L.Core.Logging.ConsoleLog;

[UnsupportedOSPlatform("browser")]
[ProviderAlias("LConsole")]
public sealed class LConsoleLoggerProvider : ILoggerProvider
{
    private readonly IDisposable? _onChangeToken;
    private LConsoleLoggerConfig _currentConfig;
    private readonly ConcurrentDictionary<string, LConsoleLogger> _loggers = new(StringComparer.OrdinalIgnoreCase);

    public LConsoleLoggerProvider(IOptionsMonitor<LConsoleLoggerConfig> config)
    {
        _currentConfig = config.CurrentValue;
        _onChangeToken = config.OnChange(updatedConfig => _currentConfig = updatedConfig);
    }

    public ILogger CreateLogger(string categoryName) => _loggers.GetOrAdd(categoryName, name => new LConsoleLogger(name, GetCurrentConfig));

    private LConsoleLoggerConfig GetCurrentConfig() => _currentConfig;

    public void Dispose()
    {
        _loggers.Clear();
        _onChangeToken?.Dispose();
    }
}