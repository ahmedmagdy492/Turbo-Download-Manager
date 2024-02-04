namespace Turbo_Download_Manager.Helpers
{
    public interface ILogger
    {
        void LogError(string message);
        void LogInfo(string message);
        void LogWarning(string message);
    }
}