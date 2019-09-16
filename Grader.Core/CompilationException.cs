using System;

namespace Grader
{

    public class ConsoleRuntimeException : Exception
    {
        public ConsoleRuntimeException(string message) : base(message)
        {
            
        }
    }
    public class MissingConsoleInputException : ConsoleRuntimeException
    {
        public MissingConsoleInputException(string message) : base(message)
        {
        }
    }
    public class CompilationException : Exception
    {
        public CompilationException(string msg) : base(msg)
        {

        }
    }
}