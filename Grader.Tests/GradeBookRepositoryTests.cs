using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using NUnit.Framework;

namespace Grader.Tests
{
    [TestFixture]
    public class GradeBookRepositoryTests
    {
        private GradeBookRepository repository;
        private Fixture fixture;
        [SetUp]
        public void Setup()
        {
            fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization() {ConfigureMembers = true, GenerateDelegates = true});
            repository = fixture.Create<GradeBookRepository>();
        }

        [Test]
        public async Task Login_Should_ReturnProjects()
        {
            var projects = await repository.StudentLogin(fixture.Create<string>(), fixture.Create<string>());
        }
    }
}