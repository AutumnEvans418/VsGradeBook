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
    public class CSharpGenerator : ICSharpGenerator
    {
        public Action Generate(IEnumerable<string> program)
        {


            Compilation test = CreateTestCompilation(program.Select(p => CSharpSyntaxTree.ParseText(p)).ToArray());



            var newTrees = test.SyntaxTrees.Select(p =>
            {
                SemanticModel model = test.GetSemanticModel(p);

                TypeInferenceRewriter reWriter = new TypeInferenceRewriter(model);
                SyntaxNode newSource = reWriter.Visit(p.GetRoot());
                return newSource.SyntaxTree;
            }).ToArray();




            var finalCompile = CreateTestCompilation(newTrees);

            var stream = new MemoryStream();
            var emitResult = finalCompile.Emit(stream);

            if (emitResult.Success)
            {
                stream.Seek(0, SeekOrigin.Begin);


                var assembly = Assembly.Load(stream.ToArray());
                return () =>
                {
                    if (assembly.EntryPoint.GetParameters().Any())
                    {
                        assembly.EntryPoint.Invoke(null, new object[] { new string[] { } });
                    }
                    else
                    {
                        assembly.EntryPoint.Invoke(null, null);
                    }
                };
                // assembly.EntryPoint.Invoke(null, new object[] { new string[] { } });

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
            MetadataReference NetStandard = MetadataReference.CreateFromFile(Assembly.Load("netstandard, Version=2.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51").Location);
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


            var core = MetadataReference.CreateFromFile(typeof(System.Linq.IQueryable<>).Assembly.Location);
            var data = MetadataReference.CreateFromFile(typeof(System.Data.DataColumn).Assembly.Location);

            MetadataReference[] references = { mscorlib, codeAnalysis, csharpCodeAnalysis, system, runtime, grader, NetStandard , core,data};

            return CSharpCompilation.Create("Test", trees, references, new CSharpCompilationOptions(OutputKind.ConsoleApplication));
        }
    }
}