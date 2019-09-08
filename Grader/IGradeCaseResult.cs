using System.Collections.Generic;

namespace Grader
{
    public interface IGradeCaseResult
    {
        IGradeCase Case { get; }
        IEnumerable<string> ActualOutput { get; }
        bool Pass { get; }
        string ErrorMessage { get; }
    }
}