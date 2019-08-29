using System;

namespace Grader
{
    public class CompilationException : Exception
    {
        public CompilationException(string msg) : base(msg)
        {

        }
    }
}