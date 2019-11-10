namespace Grader.Core.Interfaces
{
    public interface ILogger
    {
        void Log(string message);
        void Log(string message, object obj);
    }
}