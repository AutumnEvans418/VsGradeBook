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
                _logger.Log(newSource.GetText().ToString());
                return (newSource.SyntaxTree, reWriter.HasChanges);
            }).ToArray();
            if (newTrees.Any(p => p.HasChanges))
            {
                return CreateSyntaxTrees(newTrees.Select(r => r.SyntaxTree.GetText().ToString()), references);
            }
            return newTrees.Select(p => p.SyntaxTree).ToArray();
        }
        public static IEnumerable<PortableExecutableReference> CreateNetCoreReferences()
        {
            foreach (var dllFile in Directory.GetFiles(@"C:\Program Files\dotnet\sdk\NuGetFallbackFolder\microsoft.netcore.app\2.0.0\ref\netcoreapp2.0", "*.dll"))
            {
                yield return MetadataReference.CreateFromFile(dllFile);
            }
        }
        private Compilation CreateTestCompilation(SyntaxTree[] trees, IEnumerable<string> extraReferences)
        {

            var exclude = new[] {"System"};

            _logger.Log("ExtraReferences", extraReferences);
            var references = new List<MetadataReference>();
            MetadataReference grader = MetadataReference.CreateFromFile(typeof(Grader.Console).Assembly.Location);
            MetadataReference graderCore = MetadataReference.CreateFromFile(typeof(Grader.Core.Logger).Assembly.Location);
            references.Add(grader);
            references.Add(graderCore);
            if (extraReferences?.Any() == true)
            {
                foreach (var extraReference in extraReferences.Where(r=> exclude.Any(r.Contains) != true))
                {
                    references.Add(MetadataReference.CreateFromFile(extraReference));
                }
            }
            var netStandard = MetadataReference.CreateFromFile(Assembly.Load("netstandard, Version=2.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51").Location);
            var mscorlib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
            var core = MetadataReference.CreateFromFile(typeof(System.Linq.IQueryable<>).Assembly.Location);
            var data = MetadataReference.CreateFromFile(typeof(System.Data.DataColumn).Assembly.Location);
            var defaultList = new List<MetadataReference>
                    {mscorlib, netStandard, core, data};

            references = references.Union(defaultList).ToList();
            _logger.Log("References", references.Select(p => p.Display));
            var options = new CSharpCompilationOptions(OutputKind.ConsoleApplication);

            return CSharpCompilation.Create("Test", trees, references, options);
        }
    }
}