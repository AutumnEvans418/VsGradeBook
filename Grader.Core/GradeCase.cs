using System.Collections.Generic;
using System.Linq;

namespace Grader
{
    public class GradeCase : IGradeCase
    {
        public GradeCase(string input, string expectedOutputs, int caseNumber)
        {
            var parser = new GraderCaseValueInterpreter();
            CaseNumber = caseNumber;
            Inputs = parser.ParseInput(input);
            ExpectedOutputs = parser.ParseOutput(expectedOutputs);
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