using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Grader
{
    public class CSharpGenerator
    {
        public Func<Task> Generate(IEnumerable<string> program)
        {
          

            Compilation test = CreateTestCompilation(program.Select(p=> CSharpSyntaxTree.ParseText(p)).ToArray());



            var newTrees = test.SyntaxTrees.Select(p =>
            {
                SemanticModel model = test.GetSemanticModel(p);

                TypeInferenceRewriter reWriter = new TypeInferenceRewriter(model);
                SyntaxNode newSource = reWriter.Visit(p.GetRoot());
                return newSource.SyntaxTree;
            });


           

            var finalCompile = CreateTestCompilation(newTrees.ToArray());

            var stream = new MemoryStream();
            var emitResult = finalCompile.Emit(stream);

            if (emitResult.Success)
            {
                stream.Seek(0, SeekOrigin.Begin);


                var assembly = Assembly.Load(stream.ToArray());
                return async () =>
                {
                    assembly.EntryPoint.Invoke(null, new object[] {new string[] { }});
                    //var cancellationToken = new CancellationTokenSource();

                    //int timeout = 1000;
                    //var task = Task.Run(()=> 
                    //    assembly.EntryPoint.Invoke(null, new object[] { new string[] { } }), cancellationToken.Token);
                    //if (await Task.WhenAny(task, Task.Delay(timeout, cancellationToken.Token)) == task)
                    //{
                    //    // Task completed within timeout.
                    //    // Consider that the task may have faulted or been canceled.
                    //    // We re-await the task so that any exceptions/cancellation is rethrown.
                    //    await task;

                    //}
                    //else
                    //{
                    //    cancellationToken.Cancel();
                    //    // timeout/cancellation logic
                    //}

                };
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
        }
        private Compilation CreateTestCompilation(SyntaxTree[] trees)
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

            return CSharpCompilation.Create("Test", trees, references, new CSharpCompilationOptions(OutputKind.ConsoleApplication));
        }
    }
}