using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Grader
{
    public class GradeCaseResult : IGradeCaseResult
    {
        private readonly Exception _exception;

        public GradeCaseResult(IGradeCase @case, IEnumerable<string> actualOutput, Exception exception = null)
        {
            _exception = exception;
            if (exception != null)
            {
                var errorMessage = exception.Message;
                _hasErrors = true;
                ErrorMessage = $"Case {@case.CaseNumber}: " + errorMessage + "\r\n";
            }
            Case = @case;
            ActualOutput = actualOutput;
            Evaluate();
        }

        private bool _hasErrors;
        private void Evaluate()
        {
            Pass = true;
            if (_hasErrors && Case.ExpectedOutputs.Any(r=>r.Exception) != true)
            {
                Pass = false;
                return;
            }
            foreach (var caseExpectedOutput in Case.ExpectedOutputs)
            {
                var testResultFail = false;
                if (caseExpectedOutput.CaseInsensitive)
                {
                    testResultFail = ActualOutput.All(p => p?.ToLowerInvariant().Contains(caseExpectedOutput.ValueToMatch.ToLowerInvariant()) != true);
                }
                else
                {
                    testResultFail = ActualOutput.All(p => p?.Contains(caseExpectedOutput.ValueToMatch) != true);
                }

                if (_exception != null && caseExpectedOutput.Exception)
                {
                    var msg = _exception.Message;

                    testResultFail = msg.Contains(caseExpectedOutput.ValueToMatch) != true;
                }
                if (caseExpectedOutput.Regex)
                {
                    testResultFail = ActualOutput.All(p => p != null && Regex.IsMatch(p, caseExpectedOutput.ValueToMatch) != true);
                }
                if (caseExpectedOutput.Negate)
                {
                    testResultFail = !testResultFail;
                }
                if (testResultFail)
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
                else
                {
                    ErrorMessage = "";
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