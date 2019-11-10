using Grader.Core.Interfaces;
using Newtonsoft.Json;

namespace Grader.Core
{
    public class Logger : ILogger
    {
        public void Log(string message)
        {
            System.Console.WriteLine(message);
        }

        public void Log(string message, object obj)
        {
            var msg = message + $"({JsonConvert.SerializeObject(obj, Formatting.Indented)})";
            Log(msg);
        }
    }
}