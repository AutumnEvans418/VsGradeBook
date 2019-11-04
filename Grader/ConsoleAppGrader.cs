using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Grader
{
    public class ConsoleAppGrader : IConsoleAppGrader
    {
        private readonly ICSharpGenerator _cSharpGenerator;

        public ConsoleAppGrader(ICSharpGenerator cSharpGenerator)
        {
            _cSharpGenerator = cSharpGenerator;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IGradeResult> Grade(IEnumerable<string> program, IEnumerable<IGradeCase> cases, IEnumerable<string> references = null)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var caseList = cases.ToList();
            if (caseList.Any() != true)
            {
#pragma warning disable CA1303 // Do not pass literals as localized parameters
                throw new ArgumentException("Cases cannot be empty",nameof(cases));
#pragma warning restore CA1303 // Do not pass literals as localized parameters
            }
            var generator = _cSharpGenerator;
            var runProgram =generator.Generate(program, references);

            var list = new List<IGradeCaseResult>();
            for (var i=0; i < caseList.ToList().Count; i++)
            {
                Exception exception = null;
                var gradeCase = caseList[i];
                Console.Outputs.Clear();
                Console.Inputs = gradeCase.Inputs.ToList();
                //var message = "";
                try
                {
                     runProgram();
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    exception = e.InnerException ?? e;
                }


                var outputs = Console.Outputs.ToList();
                list.Add(new GradeCaseResult(gradeCase, outputs, exception));
            }

            return new GradeResult(list);
        }


        public  Task<IGradeResult> Grade(string program, IEnumerable<IGradeCase> cases)
        {
            return Grade(new[] {program}, cases);
        }
    }
}
