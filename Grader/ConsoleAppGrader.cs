using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using System.IO;
using System.Reflection;
using System.Reflection.Metadata;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using TypeInfo = Microsoft.CodeAnalysis.TypeInfo;

namespace Grader
{

    public class TypeInferenceRewriter : CSharpSyntaxRewriter
    {
        private readonly SemanticModel _model;

        public TypeInferenceRewriter(SemanticModel model)
        {
            _model = model;
        }

        public override SyntaxNode VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        {
            if (node.Declaration.Variables.Count > 1)
            {
                return node;
            }
            if (node.Declaration.Variables[0].Initializer == null)
            {
                return node;
            }

            VariableDeclaratorSyntax declarator = node.Declaration.Variables.First();
            TypeSyntax variableTypeName = node.Declaration.Type;

            ITypeSymbol variableType =
                (ITypeSymbol)_model.GetSymbolInfo(variableTypeName)
                    .Symbol;

            TypeInfo initializerInfo =
                _model.GetTypeInfo(declarator
                    .Initializer
                    .Value);

            if (variableType == initializerInfo.Type)
            {
                TypeSyntax varTypeName =
                    IdentifierName("var")
                        .WithLeadingTrivia(
                            variableTypeName.GetLeadingTrivia())
                        .WithTrailingTrivia(
                            variableTypeName.GetTrailingTrivia());

                return node.ReplaceNode(variableTypeName, varTypeName);
            }
            else
            {
                return node;
            }
        }
    }

    public class GradeResult
    {

    }
    public class ConsoleAppGrader
    {

        public ConsoleAppGrader()
        {
        }


        public GradeResult Grade(string program)
        {


            NameSyntax name = SyntaxFactory.IdentifierName("System");
            name = SyntaxFactory.QualifiedName(name, SyntaxFactory.IdentifierName("Collections"));
            name = SyntaxFactory.QualifiedName(name, SyntaxFactory.IdentifierName("Generic"));

            SyntaxTree tree = CSharpSyntaxTree.ParseText(
               program);

            var root = (CompilationUnitSyntax)tree.GetRoot();
            var oldUsing = root.Usings[1];
            var newUsing = oldUsing.WithName(name);

            root = root.ReplaceNode(oldUsing, newUsing);

            Compilation test = CreateTestCompilation(root.SyntaxTree);



            var stream = new MemoryStream();
            var emitResult = test.Emit(stream);

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
                throw new Exception(msg);
            }
            //foreach (SyntaxTree sourceTree in test.SyntaxTrees)
            //{
            //    SemanticModel model = test.GetSemanticModel(sourceTree);

            //    TypeInferenceRewriter rewriter = new TypeInferenceRewriter(model);
            //    SyntaxNode newSource = rewriter.Visit(sourceTree.GetRoot());

            //    if (newSource != sourceTree.GetRoot())
            //    {
            //        File.WriteAllText(sourceTree.FilePath, newSource.ToFullString());
            //    }
            //}

            return null;
        }

        private Compilation CreateTestCompilation(SyntaxTree tree)
        {
            
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

            MetadataReference[] references = { mscorlib, codeAnalysis, csharpCodeAnalysis, system, runtime };

            return CSharpCompilation.Create("Test", new[] {tree}, references, new CSharpCompilationOptions(OutputKind.ConsoleApplication));
        }
    }
}
