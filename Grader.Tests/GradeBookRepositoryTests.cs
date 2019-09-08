using System;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
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
            fixture.Customize(new AutoMoqCustomization() { ConfigureMembers = true, GenerateDelegates = true });

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
            var teacherName = fixture.Create<string>();
            var userName = fixture.Create<string>();
            var classCode = fixture.Create<string>();
            using (var db = fixture.Create<Func<GradeBookDbContext>>()())
            {
                var teacher = new Person() {Name = teacherName};
                db.People.Add(new Person(){IsStudent = true, Name = userName});
                db.People.Add(teacher);
                db.Classes.Add(new Class() {Teacher = teacher, Id = classCode});
                db.SaveChanges();
            }

            var projects = await repository.StudentLogin(userName, classCode);
            projects.Data.Should().BeEmpty();
        }


        [Test]
        public async Task Login_NoStudent_Should_ThrowException()
        {
            var result = await repository.StudentLogin(fixture.Create<string>(), fixture.Create<string>());

            result.Status.Should().Be(RepositoryStatus.MissingUser);
        }
    }
}