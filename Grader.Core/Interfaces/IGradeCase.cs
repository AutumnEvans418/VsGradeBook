using System.Collections.Generic;

namespace Grader
{
    public interface IGradeCase
    {
        int CaseNumber { get; set; }
        IList<string> Inputs { get; }
        IList<string> ExpectedOutputs { get; }
    }
}