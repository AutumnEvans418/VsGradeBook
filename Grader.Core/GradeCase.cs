using System;
using System.Collections.Generic;
using System.Linq;

namespace Grader
{
    public class GradeCase : IGradeCase
    {
        public GradeCase(string input, string expectedOutputs, int caseNumber)
        {
            var list = new ParserExceptionList();
            var parser = new GraderCaseValueInterpreter();
            CaseNumber = caseNumber;

            try
            {
                Inputs = parser.ParseInput(input);
            }
            catch (ParserException e)
            {
                System.Console.WriteLine(e);
                list.Add(e);
            }

            try
            {
                ExpectedOutputs = parser.ParseOutput(expectedOutputs);
            }
            catch (ParserException e)
            {
                System.Console.WriteLine(e);
                list.Add(e);
            }

            if (list.Exceptions.Any())
            {
                throw list;
            }
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