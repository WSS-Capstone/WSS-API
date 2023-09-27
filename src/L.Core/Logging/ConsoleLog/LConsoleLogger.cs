using L.Core.Extensions;
using L.Core.Logging.Models;
using Microsoft.Extensions.Logging;

namespace L.Core.Logging.ConsoleLog;

public sealed class LConsoleLogger : ILogger
{
    private readonly string _name;
    private readonly Func<LConsoleLoggerConfig> _getCurrentConfig;

    public LConsoleLogger(
        string name,
        Func<LConsoleLoggerConfig> getCurrentConfig) =>
        (_name, _getCurrentConfig) = (name, getCurrentConfig);

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default!;

    public bool IsEnabled(LogLevel logLevel) => _getCurrentConfig().LogLevelToColorMap.ContainsKey(logLevel);

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        LConsoleLoggerConfig config = _getCurrentConfig();
        if (config.EventId == 0 || config.EventId == eventId.Id)
        {
            WriteLineLog(logLevel, eventId, _name, state, exception, formatter);
        }
    }

    private void WriteLineLog<TState>(LogLevel logLevel, EventId eventId, string name, TState state, Exception? exception, Func<TState, Exception?, string> formatter, string? customEventName = null, string? customMessage = null)
    {
        ConsoleColor originalColor = Console.ForegroundColor;
        // Datetime 
        Console.ForegroundColor = ConsoleColor.Cyan;
        DateTime now = DateTime.Now;
        Console.Write($"[{now:dd/MM/yy HH:mm}]");
        Console.ForegroundColor = originalColor;
        
        LConsoleLoggerConfig config = _getCurrentConfig();
        // Level - Event
        Console.ForegroundColor = config.LogLevelToColorMap[logLevel];
        string logLevelName = Enum.GetName(typeof(LogEnum.LogLevel), logLevel) ?? "??";
        Console.Write($"[{logLevelName.CenterText(8), -8}][{eventId.Id,2}:{eventId.Name ?? customEventName,19}]");
        
        Console.ForegroundColor = originalColor;
        Console.Write($" {name} - ");
        
        Console.ForegroundColor = config.LogLevelToColorMap[logLevel];
        Console.Write(customMessage == null ? $"{formatter(state, exception)}" : $"{customMessage}");
        Console.ForegroundColor = originalColor;
        Console.WriteLine();
        if (exception != null)
        {
            WriteLineLog(logLevel, eventId, name, state, null, formatter, "Source", exception.Source);
            string[] messages = exception.Message.Split(". ");
            foreach (var m in messages)
            {
              WriteLineLog(logLevel, eventId, name, state, null, formatter, "Message", m);
            }
            WriteLineLog(logLevel, eventId, name, state, null, formatter, "StackTrace", exception.StackTrace);
            
        }
        
        
    }
}