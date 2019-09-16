using System.Collections.Generic;
using System.Linq;

namespace Grader
{
    public class GradeCaseResult : IGradeCaseResult
    {
        public GradeCaseResult(IGradeCase @case, IEnumerable<string> actualOutput, string errorMessage)
        {
            ErrorMessage = errorMessage;
            Case = @case;
            ActualOutput = actualOutput;
            Evaluate();
        }

        private void Evaluate()
        {
            Pass = true;
            if(string.IsNullOrWhiteSpace(ErrorMessage) != true)
            {
                Pass = false;
                return;
            }
            foreach (var caseExpectedOutput in Case.ExpectedOutputs)
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
        public string ErrorMessage { get; set; }
    }
}