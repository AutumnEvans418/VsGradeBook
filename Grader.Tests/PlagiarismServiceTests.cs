using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using Grader.Web;
using Moq;
using NUnit.Framework;

namespace Grader.Tests
{
    [TestFixture]
    public class PlagiarismServiceTests
    {
        private PlagiarismService plagiarismService;
        private Fixture fixture;
        private Mock<IGradeBookRepository> repository;
        [SetUp]
        public void Setup()
        {
            fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization() { ConfigureMembers = true, GenerateDelegates = true });
            repository = fixture.Freeze<Mock<IGradeBookRepository>>();
            plagiarismService = fixture.Build<PlagiarismService>().OmitAutoProperties().Create();
        }

        [TestCase("test", "tset", "test", true, false, true)]
        [TestCase("asdf", "test", "test", false, true, true)]
        [TestCase("test", "test", "test", true, true, true)]
        [TestCase("asdf", "asdf", "test", true, true, false)]
        [TestCase("", "asdf", "test", false, false, false)]
        [TestCase(@"public class test{}", @"public  class test {}", @"test", true, true, false)]
        public async Task ThreeSubmissions_Should_Be(string sub1, string sub2, string sub3, bool plag1, bool plag2, bool plag3)
        {
            var data =new []
            {
                new Submission(){SubmissionFiles = {new SubmissionFile(){Content = sub1}}, ProjectId = 1},
                new Submission(){SubmissionFiles = {new SubmissionFile(){Content = sub2}}, ProjectId = 2},
                new Submission(){SubmissionFiles = {new SubmissionFile(){Content = sub3}}, ProjectId = 3},
            };
            
            repository.Setup(p => p.GetSubmissions(It.IsAny<int>())).Returns(Task.FromResult(data.AsEnumerable()));
            
            await plagiarismService.Check(fixture.Create<int>());
            if (plag1)
            {
                repository.Verify(p => p.Plagiarized(It.Is<IEnumerable<Submission>>(r => r.Any(t => t.SubmissionFiles[0].Content == sub1))));
            }
            if (plag2)
            {
                repository.Verify(p => p.Plagiarized(It.Is<IEnumerable<Submission>>(r => r.Any(t => t.SubmissionFiles[0].Content == sub2))));
            }
            if (plag3)
            {
                repository.Verify(p => p.Plagiarized(It.Is<IEnumerable<Submission>>(r => r.Any(t => t.SubmissionFiles[0].Content == sub3))));
            }
        }
    }
}