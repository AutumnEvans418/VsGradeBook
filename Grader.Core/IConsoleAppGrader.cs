using System.Collections.Generic;
using System.Threading.Tasks;

namespace Grader
{
    public interface IConsoleAppGrader
    {
        Task<IGradeResult> Grade(IEnumerable<string> codes, IEnumerable<IGradeCase> gradeCases);
    }
}