using System.Linq;
using System.Threading.Tasks;
using AsyncToolWindowSample.Models;
using AsyncToolWindowSample.ToolWindows;
using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Grader.Tests
{
    [TestFixture]
    public class ProjectViewModelTests
    {
        private Fixture fixture;
        [SetUp]
        public void Setup()
        {
            fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization() { ConfigureMembers = true, GenerateDelegates = true });
            fixture.Inject<IConsoleAppGrader>(new ConsoleAppGrader(new CSharpGenerator()));
        }

        [Test]
        public async Task HelloWorld_Should_Pass()
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
            System.Console.WriteLine(""Hello World!"");
        }
    }
}";
            var output = "Hello World!";
            var code = fixture.Freeze<Mock<IVisualStudioService>>();

            code.Setup(p => p.GetCSharpFilesAsync()).Returns(Task.FromResult(new[] { new FileContent(){Content = src} }.AsEnumerable()));


            ProjectViewModel model = fixture.Build<ProjectViewModel>().OmitAutoProperties().Create();

            model.CodeProject.CsvExpectedOutput = output;

            await model.TestCommand.ExecuteAsync();


            model.Submission.Grade.Should().Be(1);
        }


        [Test]
        public async Task InvalidNumberOfInputs_ShouldGiveErrorMessage()
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
            var code = new Mock<IVisualStudioService>();

            code.Setup(p => p.GetCSharpFilesAsync()).Returns(Task.FromResult(new[] { new FileContent(){Content = src}}.AsEnumerable()));

            ProjectViewModel model = fixture.Build<ProjectViewModel>().OmitAutoProperties().Create();



            await model.TestCommand.ExecuteAsync();
            model.ErrorMessage.Should().Be("Case 1: Missing input\r\n");
        }

        [Test]
        public async Task InvalidNumberOfInputs_Should_NotFailAllCases()
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
            var code = new Mock<IVisualStudioService>();

            code.Setup(p => p.GetCSharpFilesAsync()).Returns(Task.FromResult(new[] { new FileContent(){Content = src}}.AsEnumerable()));
            ProjectViewModel model = fixture.Build<ProjectViewModel>().OmitAutoProperties().Create();


            model.CodeProject.CsvCases = "10\r\n12\r\ntest";
            model.CodeProject.CsvExpectedOutput = "11\r\n13.2\r\n";

            await model.Test();

            model.Submission.Grade.Should().BeInRange(.65, .67);
        }
    }
}