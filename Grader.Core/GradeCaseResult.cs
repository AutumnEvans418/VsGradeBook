using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Grader
{
    public class GradeCaseResult : IGradeCaseResult
    {
        public GradeCaseResult(IGradeCase @case, IEnumerable<string> actualOutput, string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage) != true)
            {
                _hasErrors = true;
                ErrorMessage = $"Case {@case.CaseNumber}: " + errorMessage;
            }
            Case = @case;
            ActualOutput = actualOutput;
            Evaluate();
        }

        private bool _hasErrors;
        private void Evaluate()
        {
            Pass = true;
            if (_hasErrors)
            {
                Pass = false;
                return;
            }
            foreach (var caseExpectedOutput in Case.ExpectedOutputs)
            {
                var testResult = false;
                if (caseExpectedOutput.CaseInsensitive)
                {
                    testResult = ActualOutput.All(p => p?.ToLowerInvariant().Contains(caseExpectedOutput.ValueToMatch.ToLowerInvariant()) != true);
                }
                else
                {
                    testResult = ActualOutput.All(p => p?.Contains(caseExpectedOutput.ValueToMatch) != true);
                }

                if (caseExpectedOutput.Regex)
                {
                    testResult = ActualOutput.All(p => p != null && Regex.IsMatch(p, caseExpectedOutput.ValueToMatch) != true);
                }
                if (caseExpectedOutput.Negate)
                {
                    testResult = !testResult;
                }
                if (testResult)
                {
                    Pass = false;
                    if (string.IsNullOrWhiteSpace(caseExpectedOutput.Hint) != true)
                    {
                        ErrorMessage += $"Case {Case.CaseNumber}: '{caseExpectedOutput.Hint}'\r\n";
                    }
                    var expected = "Expected";
                    if (caseExpectedOutput.Negate)
                    {
                        expected = "Not Expected";
                    }
                    ErrorMessage += $"Case {Case.CaseNumber}: {expected} '{caseExpectedOutput.ValueToMatch}'\r\n";
                }

            }

            if (!Pass)
            {
                ErrorMessage += $"Case {Case.CaseNumber}: Failed";
            }

        }

        public IGradeCase Case { get; }
        public IEnumerable<string> ActualOutput { get; private set; }
        public bool Pass { get; private set; }
        public string ErrorMessage { get; set; }
    }
}