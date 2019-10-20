using System.Collections.Generic;
using System.Linq;

namespace Grader
{

    public class GradeCase : IGradeCase
    {
        public GradeCase(string input, string expectedOutputs, int caseNumber)
        {
            CaseNumber = caseNumber;
            Inputs = ParseInput(input);
            ExpectedOutputs = ParseInput(expectedOutputs).Select(p => new CaseValue(p)).ToList();
        }

        public IList<string> ParseInput(string input)
        {
            var lst = new List<string>();

            var index = 0;
            var current = "";

            char? get()
            {
                if (index < input.Length)
                {
                    return input[index];
                }

                return null;
            }
            while (index < input.Length)
            {
                var c = get();
                if (c == '"')
                {
                    index++;
                    c = get();
                    if (string.IsNullOrWhiteSpace(current))
                    {
                        current = "";
                    }
                    while (c != '"')
                    {
                        current += c;
                        index++;
                        c = get();
                    }

                    index++;
                    c = get();

                }
                if (c == ',' || c == null)
                {
                    lst.Add(current);
                    current = "";
                    index++;
                }
                else
                {
                    current += c;
                    index++;
                }

            }

            return lst;
        }
        public GradeCase(int caseNumber)
        {
            CaseNumber = caseNumber;
        }

        public int CaseNumber { get; }
        public IList<string> Inputs { get; } = new List<string>();
        public IList<CaseValue> ExpectedOutputs { get; } = new List<CaseValue>();
    }
}