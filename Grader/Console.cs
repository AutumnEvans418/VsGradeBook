using System.Collections.Generic;
using System.Linq;

namespace Grader
{
    public static class Console
    {
        public static List<string> Outputs = new List<string>();
        public static List<string> Inputs = new List<string>();
        public static void WriteLine(object obj)
        {
            Outputs.Add(obj?.ToString());
        }
        public static string ReadLine()
        {
            if (Inputs.Any() != true)
            {
                throw new MissingConsoleInputException("Missing input");
            }
            var first = Inputs.First();
            
            Inputs.Remove(first);
            return first;
        }
    }
}