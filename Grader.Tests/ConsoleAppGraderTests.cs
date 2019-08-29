using System;
using NUnit.Framework;

namespace Grader.Tests
{
    [TestFixture]
    public class ConsoleAppGraderTests
    {
        [SetUp]
        public void Setup()
        {
            Grader.Console.Outputs.Clear();
        }
        [Test]
        public void Grade_Should_NotThrowException()
        {
            var grader = new ConsoleAppGrader();

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

            var result = grader.Grade(src);

            Assert.AreEqual(1, Grader.Console.Outputs.Count);
            Assert.Pass();
        }


        [Test]
        public void Grade_WithUsing_Should_NotThrowException()
        {
            var grader = new ConsoleAppGrader();

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

            var result = grader.Grade(src);
            Assert.AreEqual(1, Grader.Console.Outputs.Count);
            Assert.Pass();
        }
    }
}
