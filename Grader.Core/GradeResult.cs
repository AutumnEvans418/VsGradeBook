using System.Collections.Generic;
using System.Linq;

namespace Grader
{
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