using System.Threading.Tasks;
using AsyncToolWindowSample.ToolWindows;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using NUnit.Framework;

namespace Grader.Tests
{
    //[TestFixture]
    //public class LoginViewModelTests
    //{
    //    private LoginViewModel model;
    //    private Fixture fixture;
    //    private Mock<INavigationService> toolWindow;
    //    [SetUp]
    //    public void Setup()
    //    {
    //        fixture = new Fixture();
    //        fixture.Customize(new AutoMoqCustomization() { ConfigureMembers = true, GenerateDelegates = true });
    //        toolWindow = fixture.Freeze<Mock<INavigationService>>();
    //        model = fixture.Build<LoginViewModel>().OmitAutoProperties().Create();
    //    }

    //    [Test]
    //    public async Task StudentLogin_Should_Navigate()
    //    {
    //        model.IsStudent = true;
    //        model.Name = fixture.Create<string>();
    //        model.CourseCode = fixture.Create<string>();

    //        await model.LoginCommand.ExecuteAsync();

    //        toolWindow.Verify(p=>p.ToPage(It.IsAny<string>(),It.Is<INavigationParameter>(r=>r.ContainsKey("Projects"))));
    //    }
    //}
}