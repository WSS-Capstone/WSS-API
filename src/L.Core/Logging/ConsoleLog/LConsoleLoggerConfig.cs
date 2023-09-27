using Microsoft.Extensions.Logging;

namespace L.Core.Logging.ConsoleLog;

public sealed class LConsoleLoggerConfig
{
    public int EventId { get; set; }

    public Dictionary<LogLevel, ConsoleColor> LogLevelToColorMap { get; set; } = new()
    {
        [LogLevel.Information] = ConsoleColor.Green,
        [LogLevel.Error] = ConsoleColor.Red,
        [LogLevel.Warning] = ConsoleColor.Yellow,
        [LogLevel.Critical] = ConsoleColor.DarkRed,
        [LogLevel.Debug] = ConsoleColor.DarkGray,
        [LogLevel.Trace] = ConsoleColor.Gray,
        [LogLevel.None] = ConsoleColor.White
    };
}