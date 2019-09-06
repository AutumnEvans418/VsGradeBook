using System.Linq;
using System.Threading.Tasks;
using AsyncToolWindowSample.Models;
using AsyncToolWindowSample.ToolWindows;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Grader.Tests
{
    [TestFixture]
    public class ProjectViewModelTests
    {
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
            var code = new Mock<IVisualStudioService>();

            code.Setup(p => p.GetCSharpFilesAsync()).Returns(Task.FromResult(new []{src}.AsEnumerable()));


            ProjectViewModel model = new ProjectViewModel(code.Object, new ConsoleAppGrader());

            model.ExpectedOutput = output;

            await model.TestCommand.ExecuteAsync();


            model.PercentPass.Should().Be(1);
        }
    }
}