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
        private Mock<IVisualStudioService> vsMock;
        private ProjectViewModel model;
       [SetUp]
        public void Setup()
        {
            fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization() { ConfigureMembers = true, GenerateDelegates = true });
            fixture.Inject<IConsoleAppGrader>(new ConsoleAppGrader(new CSharpGenerator()));

            vsMock = fixture.Freeze<Mock<IVisualStudioService>>();
             model = fixture.Build<ProjectViewModel>().OmitAutoProperties().Create();

        }
        const string  helloWorldSrc = @"
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

        const string taxSystemSrc = @"
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
        [Test]
        public async Task HelloWorld_Should_Pass()
        {
            
            var output = "Hello World!";

            vsMock.Setup(p => p.GetCSharpFilesAsync()).Returns(Task.FromResult(new[] { new FileContent(){Content = helloWorldSrc} }.AsEnumerable()));

            model.CodeProject.CsvExpectedOutput = output;

            await model.TestCommand.ExecuteAsync();


            model.Submission.Grade.Should().Be(1);
        }


        [Test]
        public async Task InvalidNumberOfInputs_ShouldGiveErrorMessage()
        {

            vsMock.Setup(p => p.GetCSharpFilesAsync()).Returns(Task.FromResult(new[] { new FileContent(){Content = taxSystemSrc}}.AsEnumerable()));

            await model.TestCommand.ExecuteAsync();
            model.ErrorMessage.Should().Be("Case 1: Missing input\r\n");
        }


        [Test]
        public async Task NoInputOutput_Should_Pass()
        {
            vsMock.Setup(p => p.GetCSharpFilesAsync())
                .Returns(Task.FromResult(new[] {new FileContent() {Content = helloWorldSrc}}.AsEnumerable()));
            model.CodeProject.CsvExpectedOutput = null;
            model.CodeProject.CsvCases = null;
            await model.TestCommand.ExecuteAsync();

            model.ErrorMessage.Should().BeNullOrEmpty();
        }

        [Test]
        public async Task InputWithNoOutput_Should_Pass()
        {
            vsMock.Setup(p => p.GetCSharpFilesAsync())
                .Returns(Task.FromResult(new[] { new FileContent() { Content = taxSystemSrc } }.AsEnumerable()));
            model.CodeProject.CsvExpectedOutput = null;
            model.CodeProject.CsvCases = "1,2\r\n1,2";
            await model.TestCommand.ExecuteAsync();

            model.Submission.Grade.Should().Be(1);
            model.ErrorMessage.Should().BeNullOrEmpty();
        }

        [Test]
        public void InputWithoutOutput_Count_Should_BeTwo()
        {
            var result = ProjectViewModel.ConvertToGradeCases(new string[0], new[] {"1,2", "1,2"});
            result.Should().HaveCount(2);
        }



        [Test]
        public async Task InvalidNumberOfInputs_Should_NotFailAllCases()
        {
            

            vsMock.Setup(p => p.GetCSharpFilesAsync()).Returns(Task.FromResult(new[] { new FileContent(){Content = taxSystemSrc}}.AsEnumerable()));

            model.CodeProject.CsvCases = "10\r\n12\r\ntest";
            model.CodeProject.CsvExpectedOutput = "11\r\n13.2\r\n";

            await model.Test();

            model.Submission.Grade.Should().BeInRange(.65, .67);
        }
    }
}