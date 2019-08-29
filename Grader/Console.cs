using System.Collections.Generic;

namespace Grader
{
    public static class Console
    {
        public static List<string> Outputs = new List<string>();
        public static void WriteLine(object obj)
        {
            Outputs.Add(obj?.ToString());
        }
    }
}