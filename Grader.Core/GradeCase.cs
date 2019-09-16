using System.Collections.Generic;

namespace Grader
{
    public class GradeCase : IGradeCase
    {
        public GradeCase(IList<string> inputs, IList<string> expectedOutputs)
        {
            Inputs = inputs;
            ExpectedOutputs = expectedOutputs;
        }

        public GradeCase()
        {
            
        }
        public IList<string> Inputs { get; } = new List<string>();
        public IList<string> ExpectedOutputs { get; } = new List<string>();
    }
}