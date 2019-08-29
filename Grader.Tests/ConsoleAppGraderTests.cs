using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Grader.Tests
{
    [TestFixture]
    public class ConsoleAppGraderTests
    {
        ConsoleAppGrader grader ;

        [SetUp]
        public void Setup()
        {
            grader = new ConsoleAppGrader();
            Grader.Console.Outputs.Clear();
        }
        [Test]
        public void Grade_Should_NotThrowException()
        {

            var src = @"
using System.Collections;
using System.Linq;
using System.Text;
 
namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine(""Hello, World!"");
        }
    }
}";

            var result = grader.Grade(src, new List<IGradeCase>());

            Assert.AreEqual(1, Grader.Console.Outputs.Count);
            Assert.Pass();
        }

        [Test]
        public void GradeWithUsingStatic_Should_NotThrowException()
        {

            var src = @"
using System.Collections;
using System.Linq;
using System.Text;
using static System.Console;
namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteLine(""Hello, World!"");
        }
    }
}";

            var result = grader.Grade(src, new List<IGradeCase>());

            Assert.AreEqual(1, Grader.Console.Outputs.Count);
            Assert.Pass();
        }

        [Test]
        public void Grade_Should_ReturnHelloWorld()
        {
            var src = @"
using System.Collections;
using System.Linq;
using System.Text;
 
namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine(""Hello, World!"");
        }
    }
}";

            var result = grader.Grade(src, new List<IGradeCase>());


            var gCase = result.CaseResults.First();
            gCase.Pass.Should().BeTrue();
            gCase.ActualOutput.First().Should().Be("Hello, World!");
            gCase.Case.ExpectedOutputs.First().Should().Be("Hello, World!");

            result.PercentPassing.Should().Be(1);
        }

        [Test]
        public void Grade_WithUsing_Should_NotThrowException()
        {
            var src = @"using System;
using System.Collections;
using System.Linq;
using System.Text;
 
namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(""Hello, World!"");
        }
    }
}";

            var result = grader.Grade(src, new List<IGradeCase>());
            Assert.AreEqual(1, Grader.Console.Outputs.Count);
            Assert.Pass();
        }
    }
}
