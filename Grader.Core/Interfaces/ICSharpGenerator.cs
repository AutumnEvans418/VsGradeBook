using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Grader
{
    public interface ICSharpGenerator
    {
        Action Generate(IEnumerable<string> program, IEnumerable<string> references);
    }
}