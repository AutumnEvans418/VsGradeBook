using System;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
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

            var sqlcon = new SqliteConnection("DataSource=:memory:");
            sqlcon.Open();
            var options = new DbContextOptionsBuilder().UseSqlite(sqlcon).EnableSensitiveDataLogging().Options;
            fixture.Inject<Func<GradeBookDbContext>>(() =>
            {
                var db = new GradeBookDbContext(options);
                db.Database.EnsureCreated();
                return db;
            });
            
            repository = fixture.Create<GradeBookRepository>();
        }

        [Test]
        public async Task Login_Should_ReturnProjects()
        {
            var projects = await repository.StudentLogin(fixture.Create<string>(), fixture.Create<string>());
        }
    }
}