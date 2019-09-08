using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace Grader
{
    public class ConsoleAppGrader
    {

        public ConsoleAppGrader()
        {
        }

        public async Task<IGradeResult> Grade(IEnumerable<string> program, IEnumerable<IGradeCase> cases)
        {
            if (cases.Any() != true)
            {
                throw new ArgumentException("cases cannot be empty");
            }
            var generator = new CSharpGenerator();
            var runProgram = generator.Generate(program);

            var list = new List<IGradeCaseResult>();
            foreach (var gradeCase in cases)
            {
                Console.Outputs.Clear();
                Console.Inputs = gradeCase.Inputs.ToList();
                var message = "";
                try
                {
                    await runProgram();
                }
                catch (Exception e)
                {
                    message = e.InnerException?.Message;
                }


                var outputs = Console.Outputs.ToList();
                list.Add(new GradeCaseResult(gradeCase, outputs){Message = message});
            }

            return new GradeResult(list);
        }


        public  Task<IGradeResult> Grade(string program, IEnumerable<IGradeCase> cases)
        {
            return Grade(new[] {program}, cases);
        }

        private Compilation CreateTestCompilation(SyntaxTree tree)
        {

            MetadataReference runtime = MetadataReference.CreateFromFile(typeof(System.Runtime.CompilerServices.AccessedThroughPropertyAttribute).Assembly.Location);
            MetadataReference grader = MetadataReference.CreateFromFile(typeof(ConsoleAppGrader).Assembly.Location);
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
            MetadataReference system = MetadataReference.CreateFromFile(typeof(Console).Assembly.Location);
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
            MetadataReference mscorlib =
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
            MetadataReference codeAnalysis =
                MetadataReference.CreateFromFile(typeof(SyntaxTree).Assembly.Location);
            MetadataReference csharpCodeAnalysis =
                MetadataReference.CreateFromFile(typeof(CSharpSyntaxTree).Assembly.Location);

            MetadataReference[] references = { mscorlib, codeAnalysis, csharpCodeAnalysis, system, runtime, grader };

            return CSharpCompilation.Create("Test", new[] { tree }, references, new CSharpCompilationOptions(OutputKind.ConsoleApplication));
        }
    }
}
