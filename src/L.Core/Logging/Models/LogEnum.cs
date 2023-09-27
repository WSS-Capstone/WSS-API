namespace L.Core.Logging.Models;

public static class LogEnum
{
    /// <summary>
    /// Level of log.
    /// </summary>
    public enum LogLevel
    {
        Trace = 0,
        Debug = 1,
        Info = 2,
        Warn = 3,
        Error = 4,
        Critical = 5,
        None = 6,
    }
    
    public enum LogEventType
    {
        Information = 100,
        Warning = 200,
        Error = 300,
        Security = 400,
        Audit = 500,
        Performance = 600
    }
    
    
}