namespace PMSCore.Test;

using Microsoft.Extensions.Logging;

/// <summary>
/// Fake class to controller logger dependency injection
/// </summary>
public class LoggerFake : ILogger
{
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        LogStream += state?.ToString();
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        throw new NotImplementedException();
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        throw new NotImplementedException();
    }

    public string LogStream { get; private set; } = string.Empty;
}