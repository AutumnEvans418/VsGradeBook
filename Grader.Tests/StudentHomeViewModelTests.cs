using AsyncToolWindowSample.ToolWindows;
using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using NUnit.Framework;

namespace Grader.Tests
{
    [TestFixture]
    public class StudentHomeViewModelTests
    {
        private StudentHomeViewModel model;
        private Fixture fixture;
        [SetUp]
        public void Setup()
        {
            fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization() {ConfigureMembers = true, GenerateDelegates = true});
            model = fixture.Build<StudentHomeViewModel>().OmitAutoProperties().Create();
        }


        [Test]
        public void Lists_Should_BeEmpty()
        {
            model.InProgressList.Should().BeEmpty();
            model.SubmittedList.Should().BeEmpty();
            model.ToDoList.Should().BeEmpty();
        }

        [Test]
        public void Initialize_ShouldSetLists()
        {
            model.Initialize(new NavigationParameter(){{"Projects", fixture.CreateMany<StudentProjectSummaryDto>()}});

            model.InProgressList.Should().NotBeEmpty();
            model.SubmittedList.Should().NotBeEmpty();
            model.ToDoList.Should().NotBeEmpty();
        }
    }
}