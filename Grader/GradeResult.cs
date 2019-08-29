using System.Collections.Generic;
using System.Linq;

namespace Grader
{
    public class GradeCase : IGradeCase
    {
        public GradeCase(IEnumerable<string> inputs, IEnumerable<string> expectedOutputs)
        {
            Inputs = inputs;
            ExpectedOutputs = expectedOutputs;
        }

        public IEnumerable<string> Inputs { get; }
        public IEnumerable<string> ExpectedOutputs { get; }
    }

    public class GradeCaseResult : IGradeCaseResult
    {
        public GradeCaseResult(IGradeCase @case)
        {
            Case = @case;
        }

        public IGradeCase Case { get; }
        public IEnumerable<string> ActualOutput { get; set; }
        public bool Pass { get; set; }
        public string Message { get; set; }
    }

    public class GradeResult : IGradeResult
    {
        public GradeResult(IEnumerable<IGradeCaseResult> caseResults)
        {
            CaseResults = caseResults ?? new List<IGradeCaseResult>();
        }

        public double PercentPassing => CaseResults.Count(p => p.Pass) * 1.0 / CaseResults.Count();
        public IEnumerable<IGradeCaseResult> CaseResults { get; }
    }
}