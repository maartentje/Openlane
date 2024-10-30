using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace Shared.Extensions;

public static class RegisterLogging
{
    public static void AddSharedLogger(this ILoggingBuilder logging)
    {
        logging
            .AddConsole((options => { options.FormatterName = LogFormatter.FormatterName; }))
            .AddConsoleFormatter<LogFormatter, SimpleConsoleFormatterOptions>();
    }
}