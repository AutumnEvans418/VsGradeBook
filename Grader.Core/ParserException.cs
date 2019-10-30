using System;
using System.Collections.Generic;
using System.Linq;

namespace Grader
{
    public class ParserException : Exception
    {
        public ParserException() 
        {
            
        }

        public ParserException(string msg) : base(msg)
        {
            
        }
        public List<Exception> Exceptions { get; set; } = new List<Exception>();

        public override string ToString()
        {
            return $"{(Exceptions.Any() ? Exceptions.Select(p=> p.ToString()).Aggregate((f,s) => f + "\r\n" + s) : "")}\r\n" + base.ToString();
        }
    }
}