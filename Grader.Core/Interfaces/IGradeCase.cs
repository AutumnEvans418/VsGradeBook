using System.Collections.Generic;

namespace Grader
{
    public interface IGradeCase
    {
        int CaseNumber { get;  }
        IList<string> Inputs { get; }
        IList<CaseValue> ExpectedOutputs { get; }
    }
}