using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;

namespace Grader
{
    public class ConsoleAppGrader
    {

        public ConsoleAppGrader()
        {
        }


        public IGradeResult Grade(string program, IEnumerable<IGradeCase> cases)
        {


            //NameSyntax name = SyntaxFactory.IdentifierName("System");
            //name = SyntaxFactory.QualifiedName(name, SyntaxFactory.IdentifierName("Collections"));
            //name = SyntaxFactory.QualifiedName(name, SyntaxFactory.IdentifierName("Generic"));

            SyntaxTree tree = CSharpSyntaxTree.ParseText(
               program);

            var root = (CompilationUnitSyntax)tree.GetRoot();
            //var oldUsing = root.Usings[1];
            //var newUsing = oldUsing.WithName(name);

            //root = root.ReplaceNode(oldUsing, newUsing);

            Compilation test = CreateTestCompilation(root.SyntaxTree);



            var newTree = test.SyntaxTrees.First();


            SemanticModel model = test.GetSemanticModel(newTree);

            TypeInferenceRewriter reWriter = new TypeInferenceRewriter(model);
            SyntaxNode newSource = reWriter.Visit(newTree.GetRoot());

            var finalCompile = CreateTestCompilation(newSource.SyntaxTree);

            var stream = new MemoryStream();
            var emitResult = finalCompile.Emit(stream);

            if (emitResult.Success)
            {
                stream.Seek(0, SeekOrigin.Begin);


                //var assembly = Mono.Cecil.AssemblyDefinition.ReadAssembly(stream);

                //var ass = assembly.Modules.First();
                var assembly = Assembly.Load(stream.ToArray());
                assembly.EntryPoint.Invoke(null, new object[] { new string[] { } });
            }
            else
            {
                var msg = "";
                foreach (var emitResultDiagnostic in emitResult.Diagnostics)
                {
                    msg += emitResultDiagnostic.GetMessage() + "\r\n";
                }
                throw new CompilationException(msg);
            }


            return null;
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

    public interface IGradeCaseResult
    {
        IGradeCase Case { get; }
        IEnumerable<string> ActualOutput { get; }
        bool Pass { get; }
        string Message { get; }
    }

    public interface IGradeCase
    {
        IEnumerable<string> Inputs { get; }
        IEnumerable<string> ExpectedOutputs { get; }
    }

    public interface IGradeResult
    {
        double PercentPassing { get; }

        IEnumerable<IGradeCaseResult> CaseResults { get; }
    }
}
