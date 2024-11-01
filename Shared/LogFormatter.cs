using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;

namespace Shared;

/// <summary>
/// Uses a custom format so every (non-exception) message is prefixed with e.g. [0000-00-00 00:00:00 - Information] [EventProcessor] 
/// </summary>
public sealed class LogFormatter() : ConsoleFormatter(FormatterName)
{
    public const string FormatterName = "Custom";
    
    public override void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider? scopeProvider, TextWriter textWriter)
    {
        var date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        var logLevel = logEntry.LogLevel.ToString();
        var service = Assembly.GetEntryAssembly()?.GetName()?.Name ?? string.Empty;
        var message = logEntry.Formatter(logEntry.State, logEntry.Exception);
        
        textWriter.WriteLine($"[{date} - {logLevel}] [{service}] {message}");
        if (logEntry.Exception is not null)
        {
            textWriter.WriteLine(logEntry.Exception);
        }
    }
}