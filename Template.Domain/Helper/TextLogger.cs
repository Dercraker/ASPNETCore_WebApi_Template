using Microsoft.Extensions.Logging;

namespace Template.Domain.Helper;

public class TextLogger : ILogger
{
    public IDisposable BeginScope<TState>(TState state) => throw new NotImplementedException();

    public bool IsEnabled(LogLevel logLevel) => (int)logLevel > (int)LogLevel.Information;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) => Console.WriteLine($"{DateTime.Now:u} : {logLevel} - {formatter.Invoke(state, null)} {exception?.Message}");

}
