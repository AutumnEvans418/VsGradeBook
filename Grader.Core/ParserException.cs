using System;
using System.Collections.Generic;
using System.Linq;

namespace Grader
{
    public class ParserExceptionList : ParserException
    {
        private readonly IList<Exception> _exceptions = new List<Exception>();
        public IEnumerable<Exception> Exceptions => _exceptions;
        public void Add(Exception ex)
        {
            if (ex is ParserExceptionList list)
            {
                foreach (var listException in list.Exceptions)
                {
                    _exceptions.Add(listException);
                }
            }
            else
            {
                _exceptions.Add(ex);
            }
        }
        public override string ToString()
        {
            return $"{(Exceptions.Any() ? Exceptions.Select(p => p.ToString()).Aggregate((f, s) => f + "\r\n" + s) : "")}\r\n" + base.ToString();
        }
    }
    public class ParserException : Exception
    {
        public ParserException()
        {

        }

        public ParserException(string msg) : base(msg)
        {

        }


    }
}