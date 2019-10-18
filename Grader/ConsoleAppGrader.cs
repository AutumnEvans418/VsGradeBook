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

        public async Task<IGradeResult> Grade(IEnumerable<string> program, IEnumerable<IGradeCase> cases)
        {
            var caseList = cases.ToList();
            if (caseList.Any() != true)
            {
                throw new ArgumentException("cases cannot be empty");
            }
            var generator = _cSharpGenerator;
            var runProgram =generator.Generate(program);

            var list = new List<IGradeCaseResult>();
            for (var i=0; i < caseList.ToList().Count; i++)
            {
                var gradeCase = caseList[i];
                Console.Outputs.Clear();
                Console.Inputs = gradeCase.Inputs.ToList();
                var message = "";
                try
                {
                     runProgram();
                }
                catch (Exception e)
                {
                    message =  e.InnerException?.Message;
                }


                var outputs = Console.Outputs.ToList();
                list.Add(new GradeCaseResult(gradeCase, outputs, message));
            }

            return new GradeResult(list);
        }


        public  Task<IGradeResult> Grade(string program, IEnumerable<IGradeCase> cases)
        {
            return Grade(new[] {program}, cases);
        }
    }
}
