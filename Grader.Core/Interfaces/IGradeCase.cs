using System.Collections.Generic;

namespace Grader
{
    public interface IGradeCase
    {
        IList<string> Inputs { get; }
        IList<string> ExpectedOutputs { get; }
    }
}