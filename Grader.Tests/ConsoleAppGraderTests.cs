using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Grader.Tests
{
    [TestFixture]
    public class ConsoleAppGraderTests
    {
        ConsoleAppGrader grader ;
        private GradeCase gradeCase;
        [SetUp]
        public void Setup()
        {
            gradeCase = new GradeCase(new string[0], new[] {"Hello, World!"});
            grader = new ConsoleAppGrader();
            Grader.Console.Outputs.Clear();
        }


        [Test]
        public async Task MultipleFiles_Should_NotThrowException()
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
            var t = new Test();
            System.Console.WriteLine(""Hello, World!"");
        }
    }
}";
            var src2 = @"
namespace HelloWorld
{
    public class Test
    {
       
    }
}";

            var result = await grader.Grade(new []{src, src2}, new List<IGradeCase>() { gradeCase });

            Assert.AreEqual(1, Grader.Console.Outputs.Count);
            Assert.Pass();
        }

        [Test]
        public async Task WithinFuntion_Should_ChangeToGrader()
        {
            var src = @"
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var price = double.Parse(Console.ReadLine());
        }
    }
}
";
            var result = await grader.Grade(src, new[] { new GradeCase(new List<string>(){"0"},new List<string>() ), });

        }

        [Test]
        public async Task EmptyGradeCase_Should_HaveErrorMessage()
        {
            var src = @"
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(""Enter Price"");
            var price = double.Parse(Console.ReadLine());

            Console.WriteLine((price * 1.10));
        }
    }
}
";

            var result = await grader.Grade(src, new[] { new GradeCase(), });

            result.CaseResults.First().ErrorMessage.Should().Be("Case 1: Missing input");
        }
        [Test]
        public async Task Grade_Should_NotThrowException()
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

            var result = await grader.Grade(src, new List<IGradeCase>(){gradeCase});

            Assert.AreEqual(1, Grader.Console.Outputs.Count);
            Assert.Pass();
        }

        private const string HelloWorld = @"
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
        [Test]
        public async Task GradeWithUsingStatic_Should_NotThrowException()
        {

            var src = HelloWorld;

            var result = await grader.Grade(src, new List<IGradeCase>(){gradeCase});

            Assert.AreEqual(1, Grader.Console.Outputs.Count);
            Assert.Pass();
        }


        [Test]
        public async Task Grade_NoCases_ThrowsException()
        {
            Assert.ThrowsAsync<ArgumentException>(()=> grader.Grade(HelloWorld, new List<IGradeCase>()));
        }

        [Test]
        public async Task Grade_Should_ReturnHelloWorld()
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

            var result = await grader.Grade(src, new List<IGradeCase>(){gradeCase});


            var gCase = result.CaseResults.First();
            gCase.Pass.Should().BeTrue();
            gCase.ActualOutput.First().Should().Be("Hello, World!");
            gCase.Case.ExpectedOutputs.First().Should().Be("Hello, World!");

            result.PercentPassing.Should().Be(1);
        }

        [Test]
        public async Task Grade_WithUsing_Should_NotThrowException()
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

            var result = await grader.Grade(src, new List<IGradeCase>(){gradeCase});
            Assert.AreEqual(1, Grader.Console.Outputs.Count);
            Assert.Pass();
        }



        [Test]
        public async Task Grade_CalculateTaxExample_PercentPass_Should_50()
        {
            var src = @"
namespace Grader.Tests
{
    public static class TaxProgram
    {
        public static void Main(string[] args)
        {
            System.Console.WriteLine(""Enter the cost(ex 12.12)"");
            var cost = System.Console.ReadLine();

            var costNum = double.Parse(cost);

            System.Console.WriteLine(""Enter the tax percent (ex. 10)"");
            var tax = System.Console.ReadLine();

            var taxNum = double.Parse(tax)/100.0;

            System.Console.WriteLine($""Original Cost: "" + costNum);
            System.Console.WriteLine($""Tax Cost: "" + (costNum * taxNum));
            System.Console.WriteLine($""Final Cost: "" + (costNum + (costNum * taxNum)));

        }
    }
}
";
            var list = new List<GradeCase>();
            {
                var taxCase = new GradeCase();
                taxCase.Inputs.Add("10");
                taxCase.Inputs.Add("10");
                taxCase.ExpectedOutputs.Add("10");
                taxCase.ExpectedOutputs.Add("1");
                taxCase.ExpectedOutputs.Add("11");
                list.Add(taxCase);
            }
            {
                var taxCase = new GradeCase();
                taxCase.Inputs.Add("10");
                taxCase.Inputs.Add("12");
                taxCase.ExpectedOutputs.Add("10");
                taxCase.ExpectedOutputs.Add("1");
                taxCase.ExpectedOutputs.Add("11.123");
                list.Add(taxCase);
            }




            var result = await grader.Grade(src, list);

            result.PercentPassing.Should().Be(.5);
        }
    }
}


