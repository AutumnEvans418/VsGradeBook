using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace Grader
{
    public class GradeBookRepository : IGradeBookRepository
    {

        public async Task<IEnumerable<StudentProject>> StudentLogin(string userName, string classCode)
        {
            throw new NotImplementedException();
        }
    }
    public interface IGradeBookRepository
    {
        
    }

    public class StudentProject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public DateTimeOffset? DateGraded { get; set; }

        public bool IsBeingGraded { get; set; }
        public string Status { get; set; }
    }
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsStudent { get; set; }
    }

    public class Enrollment
    {
        public int Id { get; set; }
        public string ClassId { get; set; }
        public int StudentId { get; set; }
        public virtual Class Class { get; set; }
        public virtual Person Student { get; set; }
    }

    public class Class
    {
        public string Id { get; set; }
        public int TeacherId { get; set; }
        public virtual Person Teacher { get; set; }
    }

    public class Submission
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int StudentId { get; set; }
        public bool IsSubmitted { get; set; }

        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? SubmissionDate { get; set; }
        public virtual CodeProject CodeProject { get; set; }
        public virtual Person Student { get; set; }
    }
    public class CodeProject
    {
        public int Id { get; set; }
        public string ClassId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CsvCases { get; set; }
        public string CsvExpectedOutput { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public bool IsPublished { get; set; }
    }

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
                await runProgram();


                var outputs = Console.Outputs.ToList();
                list.Add(new GradeCaseResult(gradeCase, outputs));
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
