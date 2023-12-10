using Microsoft.Extensions.Logging;
using PlayTen.BLL.Interfaces;

namespace PlayTen.BLL.Services
{
    public class LoggerService<T> : ILoggerService<T>
    {
        protected readonly ILogger<T> Logger;
        public LoggerService(ILogger<T> logger)
        {
            Logger = logger;
        }
        public void LogInformation(string message)
        {
            Logger.Log(LogLevel.Information, $"{message}");
        }
        public void LogWarning(string message)
        {
            Logger.Log(LogLevel.Warning, $"{message}");
        }
        public void LogTrace(string message)
        {
            Logger.Log(LogLevel.Trace, $"{message}");
        }
        public void LogDebug(string message)
        {
            Logger.Log(LogLevel.Debug, $"{message}");
        }
        public void LogError(string message)
        {
            Logger.Log(LogLevel.Error, $"{message}");
        }
    }
}
