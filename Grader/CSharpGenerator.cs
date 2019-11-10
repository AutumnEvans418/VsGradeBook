using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Grader.Core.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Grader
{
    public class CSharpGenerator : ICSharpGenerator
    {
        private readonly ILogger _logger;

        public CSharpGenerator(ILogger logger)
        {
            _logger = logger;
        }

        public Action Generate(IEnumerable<string> program, IEnumerable<string> references)
        {


            var newTrees = CreateSyntaxTrees(program, references);


            var finalCompile = CreateTestCompilation(newTrees, references);

            using (var stream = new MemoryStream())
            {
                var emitResult = finalCompile.Emit(stream);

                if (emitResult.Success)
                {
                    stream.Seek(0, SeekOrigin.Begin);


                    var assembly = Assembly.Load(stream.ToArray());
                    return () =>
                    {
                        if (assembly.EntryPoint.GetParameters().Any())
                        {
                            assembly.EntryPoint.Invoke(null, new object[] { Array.Empty<string>() });
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
           
        }

        public SyntaxTree[] CreateSyntaxTrees(IEnumerable<string> program, IEnumerable<string> references)
        {
            Compilation test = CreateTestCompilation(program.Select(p => CSharpSyntaxTree.ParseText(p)).ToArray(), references);


            var newTrees = test.SyntaxTrees.Select(p =>
            {
                SemanticModel model = test.GetSemanticModel(p);
                TypeInferenceRewriter reWriter = new TypeInferenceRewriter(model);
                SyntaxNode newSource = reWriter.Visit(p.GetRoot());
                System.Console.WriteLine(newSource.GetText());
                return (newSource.SyntaxTree, reWriter.HasChanges);
            }).ToArray();
            if (newTrees.Any(p => p.HasChanges))
            {
                return CreateSyntaxTrees(newTrees.Select(r=>r.SyntaxTree.GetText().ToString()), references);
            }
            return newTrees.Select(p=>p.SyntaxTree).ToArray();
        }

        private Compilation CreateTestCompilation(SyntaxTree[] trees, IEnumerable<string> extraReferences)
        {
            _logger.Log("ExtraReferences",extraReferences);
            var references = new List<MetadataReference>();
            MetadataReference grader = MetadataReference.CreateFromFile(typeof(ConsoleAppGrader).Assembly.Location);
            references.Add(grader);
            if (extraReferences != null)
            {
                foreach (var extraReference in extraReferences)
                {
                    references.Add(MetadataReference.CreateFromFile(extraReference));
                }
            }
            else
            {
                MetadataReference NetStandard = MetadataReference.CreateFromFile(Assembly.Load("netstandard, Version=2.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51").Location);
                MetadataReference runtime = MetadataReference.CreateFromFile(typeof(System.Runtime.CompilerServices.AccessedThroughPropertyAttribute).Assembly.Location);
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

                 references = new List<MetadataReference> { mscorlib, codeAnalysis, csharpCodeAnalysis, system, runtime, NetStandard, core, data };
            }
            _logger.Log("References", references.Select(p=>p.Display));
            var options = new CSharpCompilationOptions(OutputKind.ConsoleApplication);
            
            return CSharpCompilation.Create("Test", trees, references, options);
        }
    }
}