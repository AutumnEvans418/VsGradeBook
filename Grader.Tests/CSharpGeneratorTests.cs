using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Grader.Tests
{
    [TestFixture]
    public class CSharpGeneratorTests
    {
        private CSharpGenerator generator;

        [SetUp]
        public void Setup()
        {
            generator  = new CSharpGenerator();
        }

        [Test]
        public void Comment_Should_NotBeBetweenNamespaceAndClass()
        {
            var src = @"
using System;
    class Program
    {
        static void Main(string[] args)
        {
            //test
            Console.WriteLine(""Hello World!"");
        }

    }
";
            var tree = generator.CreateSyntaxTrees(new[] {src }, null).First();

            tree.GetText().ToString().Should().Contain("Grader.Console.WriteLine");

        }


        [Test]
        public void WriteLineWithReadLine_Should_UseGrader()
        {
            var tree = generator.CreateSyntaxTrees(new[] { ConsoleAppGraderTests.writeLineReadLineSrc }, null).First();

            tree.GetText().ToString().Should().Contain("Grader.Console.WriteLine(Grader.Console.ReadLine())");
        }

    }
}