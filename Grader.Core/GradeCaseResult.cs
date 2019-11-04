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


        bool NotMatch(bool exact, string first, string second)
        {
            if (exact)
            {
                return first?.Equals(second) != true;
            }
            return first?.Contains(second) != true;
        }

        bool CaseSensitivity(CaseValue value, string first, string second)
        {
            if (value.CaseInsensitive)
            {
                return NotMatch(value.ExactMatch, first?.ToLowerInvariant(), second?.ToLowerInvariant());
            }
            return NotMatch(value.ExactMatch, first, second);
        }

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

                if (caseExpectedOutput.Exception)
                {
                    var msg = _exception?.Message;
                    testResultFail = CaseSensitivity(caseExpectedOutput, msg, matchingValue);
                }
                else if (caseExpectedOutput.MatchOutputIndex == null)
                {
                    testResultFail = ActualOutput.All(p => CaseSensitivity(caseExpectedOutput, p, matchingValue));
                }
                else if (caseExpectedOutput.MatchOutputIndex is int i)
                {
                    var output = ActualOutput.ToList();
                    if (i < output.Count)
                    {
                        testResultFail = CaseSensitivity(caseExpectedOutput, output[i], matchingValue);
                    }
                    else
                    {
                        testResultFail = true;
                        ErrorMessage += $"Case {Case.CaseNumber}: 'Expected {i+1} outputs but there was only {output.Count} outputs\r\n";
                    }
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

                    var index = "";
                    if (caseExpectedOutput.MatchOutputIndex is int i)
                    {
                        index = $" at output index {i}";
                    }
                    if (caseExpectedOutput.Exception)
                    {
                        ErrorMessage += $"Case {Case.CaseNumber}: {expected} Exception('{caseExpectedOutput.ValueToMatch}')\r\n";
                    }
                    else
                    {
                        ErrorMessage += $"Case {Case.CaseNumber}: {expected} '{caseExpectedOutput.ValueToMatch}'{index}\r\n";
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