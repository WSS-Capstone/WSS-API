using Microsoft.Extensions.Logging;

namespace L.Core.Logging.Models;

public class LogEntry
{
    public DateTimeOffset Timestamp { get; set; }
    public LogLevel LogLevel { get; set; }
    public string Message { get; set; }
    public string LoggerName { get; set; }
    public string Exception { get; set; }

    public LogEntry(DateTimeOffset timestamp, LogLevel logLevel, string message, string loggerName, Exception exception)
    {
        Timestamp = timestamp;
        LogLevel = logLevel;
        Message = message;
        LoggerName = loggerName;
        Exception = exception?.ToString();
    }
}