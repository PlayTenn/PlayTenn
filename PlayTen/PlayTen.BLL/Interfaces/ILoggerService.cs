namespace PlayTen.BLL.Interfaces
{
    public interface ILoggerService<T>
    {
        void LogInformation(string message);
        void LogWarning(string message);
        void LogTrace(string message);
        void LogDebug(string message);
        void LogError(string message);
    }
}
