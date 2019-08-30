using System.Collections.Generic;
using System.Linq;

namespace Grader
{
    public class GradeCaseResult : IGradeCaseResult
    {
        public GradeCaseResult(IGradeCase @case, IEnumerable<string> actualOutput)
        {
            Case = @case;
            ActualOutput = actualOutput;
            Pass = true;
            foreach (var caseExpectedOutput in @case.ExpectedOutputs)
            {
                if (ActualOutput.All(p => p.Contains(caseExpectedOutput) != true))
                {
                    Pass = false;
                }
               
            }
        }

        public IGradeCase Case { get; }
        public IEnumerable<string> ActualOutput { get; private set; }
        public bool Pass { get; private set; }
        public string Message { get; set; }
    }
}