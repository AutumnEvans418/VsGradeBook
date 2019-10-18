using System.Collections.Generic;
using System.Linq;

namespace Grader
{

    public class GradeCase : IGradeCase
    {
        public GradeCase(IList<string> inputs, IList<string> expectedOutputs, int caseNumber)
        {
            CaseNumber = caseNumber;
            Inputs = inputs;
            ExpectedOutputs = expectedOutputs.Select(p=> new CaseValue(p)).ToList();
        }

        public GradeCase(int caseNumber)
        {
            CaseNumber = caseNumber;
        }

        public int CaseNumber { get; }
        public IList<string> Inputs { get; } = new List<string>();
        public IList<CaseValue> ExpectedOutputs { get; } = new List<CaseValue>();
    }
}