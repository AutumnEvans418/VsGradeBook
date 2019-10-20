//using System;
//using System.Threading.Tasks;
//using AutoFixture;
//using AutoFixture.AutoMoq;
//using FluentAssertions;
//using Microsoft.Data.Sqlite;
//using Microsoft.EntityFrameworkCore;
//using NUnit.Framework;

//namespace Grader.Tests
//{



//    [TestFixture]
//    public class GradeBookRepositoryTests
//    {
//        private GradeBookRepositoryDb _repositoryDb;
//        private Fixture fixture;
//        [SetUp]
//        public void Setup()
//        {
//            fixture = new Fixture();
//            fixture.Customize(new AutoMoqCustomization() { ConfigureMembers = true, GenerateDelegates = true });

//            var sqlcon = new SqliteConnection("DataSource=:memory:");
//            sqlcon.Open();
//            var options = new DbContextOptionsBuilder().UseSqlite(sqlcon).EnableSensitiveDataLogging().Options;
//            fixture.Inject<Func<GradeBookDbContext>>(() =>
//            {
//                var db = new GradeBookDbContext(options);
//                db.Database.EnsureCreated();
//                return db;
//            });

//            _repositoryDb = fixture.Create<GradeBookRepositoryDb>();
//        }

  


      

//        [Test]
//        public async Task AddProject()
//        {
//            var name = "cevans";

//            var project = await _repositoryDb.CreateProject(new CodeProject()
//            {
//                Name = "test",
//                CsvCases = "10\r\n12",
//                CsvExpectedOutput = "11\r\n13.2",
//                Description = "Create an app that takes one parameter: the price of the item.  Then, add a 10% tax to the item and display the final value",
//                DueDate = DateTimeOffset.Now.AddDays(1),
//            });

//            project.Id.Should().NotBe(0);
//        }

//        [Test]
//        public async Task AddSubmission()
//        {
//            var name = "cevans";

//            var project = await _repositoryDb.CreateProject(new CodeProject()
//            {
//                Name = "test",
//                CsvCases = "10\r\n12",
//                CsvExpectedOutput = "11\r\n13.2",
//                Description = "Create an app that takes one parameter: the price of the item.  Then, add a 10% tax to the item and display the final value",
//                DueDate = DateTimeOffset.Now.AddDays(1),
//            });


//            var submission = await _repositoryDb.CreateSubmission(new Submission()
//            {
//                ProjectId = project.Id,
//                StudentName = "Chris"
//            });

//            submission.Id.Should().NotBe(0);
//        }

   
    
//    }
//}