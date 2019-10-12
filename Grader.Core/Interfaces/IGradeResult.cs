using System.Collections.Generic;

namespace Grader
{
    public interface IGradeResult
    {
        double PercentPassing { get; }

        IEnumerable<IGradeCaseResult> CaseResults { get; }
    }
}