using System.Collections.Generic;
using System.Linq;

namespace Grader
{

    public class GradeCase : IGradeCase
    {
        public GradeCase(IList<string> inputs, IList<string> expectedOutputs)
        {
            Inputs = inputs;
            ExpectedOutputs = expectedOutputs.Select(p=> new CaseValue(p)).ToList();
        }

        public GradeCase()
        {
            
        }

        public int CaseNumber { get; set; }
        public IList<string> Inputs { get; } = new List<string>();
        public IList<CaseValue> ExpectedOutputs { get; } = new List<CaseValue>();
    }
}