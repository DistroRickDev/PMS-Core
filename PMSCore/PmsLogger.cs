namespace PMSCore;

public class PmsLogger : ILogger
{
    public PmsLogger(string handler, StreamWriter loggerBuffer)
    {
        _handler = handler;
        _loggerBuffer = loggerBuffer;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= LogLevel.Debug;
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception exception,
        Func<TState, Exception, string> formatter)
    {
        // Ensure that only information level and higher logs are recorded
        if (!IsEnabled(logLevel))
        {
            return;
        }

        // Get the formatted log message
        var message = formatter(state, exception);

        //Write log messages to text file
        _loggerBuffer.WriteLine($"[{DateTime.UtcNow}] [{logLevel}] [{_handler}] {message}");
        _loggerBuffer.Flush();
    }

    private readonly string _handler;
    private readonly StreamWriter _loggerBuffer;
}