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
                ErrorMessage = $"Case {@case.CaseNumber}: {exception.GetType().Name}('{errorMessage}')\r\n";
            }
            Case = @case;
            ActualOutput = actualOutput;
            Evaluate();
        }

        private bool _hasErrors;
        private void Evaluate()
        {
            Pass = true;
            if (_hasErrors && Case.ExpectedOutputs.Any(r => r.Exception) != true)
            {
                Pass = false;
            }
            foreach (var caseExpectedOutput in Case.ExpectedOutputs)
            {
                var matchingValue = caseExpectedOutput.ValueToMatch;
                var testResultFail = false;
                Func<string, string, bool> get = (s, s1) => s?.Contains(s1) != true;
                if (caseExpectedOutput.CaseInsensitive)
                {
                    get = (s, s1) => s?.ToLowerInvariant()?.Contains(s1.ToLowerInvariant()) != true;
                }

                if (caseExpectedOutput.Exception)
                {
                    var msg = _exception?.Message;
                    testResultFail = get(msg, matchingValue);
                }
                else
                {
                    testResultFail = ActualOutput.All(p => get(p, matchingValue));
                }
                if (caseExpectedOutput.Regex)
                {
                    var options = caseExpectedOutput.CaseInsensitive ? RegexOptions.IgnoreCase : RegexOptions.None;
                    testResultFail = ActualOutput.All(p => p != null && Regex.IsMatch(p, caseExpectedOutput.ValueToMatch, options) != true);
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

                    if (caseExpectedOutput.Exception)
                    {
                        ErrorMessage += $"Case {Case.CaseNumber}: {expected} Exception('{caseExpectedOutput.ValueToMatch}')\r\n";
                    }
                    else
                    {
                        ErrorMessage += $"Case {Case.CaseNumber}: {expected} '{caseExpectedOutput.ValueToMatch}'\r\n";
                    }
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