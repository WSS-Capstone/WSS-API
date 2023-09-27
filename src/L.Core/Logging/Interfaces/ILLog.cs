using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace L.Core.Logging.Interfaces;

public interface ILLog
{
    void Log(LogLevel logLevel, string message);
    void Log(LogLevel logLevel, string message, Exception exception);
    void Log(LogLevel logLevel, string context, object? jsonObject, [CallerLineNumber] int currentLine = 0, string? customMessage = null, string? customValue = null);
    void Log(LogLevel logLevel, string context, object? jsonObject, Exception exception, [CallerLineNumber] int currentLine = 0, string? customMessage = null, string? customValue = null);
    
    void LogToFile(LogLevel logLevel, [CallerLineNumber] int currentLine = 0, string? context = null, int eventId = 0, object? jsonObject = null, string? filePath = null, string? customMessage = null, string? customValue = null);
    void LogToFile(LogLevel logLevel, Exception exception, [CallerLineNumber] int currentLine = 0, string? context = null, int eventId = 0, object? jsonObject = null, string? filePath = null, string? customMessage = null, string? customValue = null);

    void LogToConsole(LogLevel logLevel, [CallerLineNumber] int currentLine = 0, string? context = null, int eventId = 0, object? jsonObject = null, string? customMessage = null, string? customValue = null);
    void LogToConsole(LogLevel logLevel, Exception exception, [CallerLineNumber] int currentLine = 0, string? context = null, int eventId = 0, object? jsonObject = null, string? customMessage = null, string? customValue = null);
}