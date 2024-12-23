namespace PMSCore;

public class PmsLoggerProvider : ILoggerProvider
{
    public PmsLoggerProvider(string logFilePath)
    {
        _logFileBuffer = new StreamWriter(logFilePath);
    }

    public ILogger CreateLogger(string handlerName)
    {
        return new PmsLogger(handlerName, _logFileBuffer);
    }

    public void Dispose()
    {
        _logFileBuffer.Dispose();
    }

    private readonly StreamWriter _logFileBuffer;
}