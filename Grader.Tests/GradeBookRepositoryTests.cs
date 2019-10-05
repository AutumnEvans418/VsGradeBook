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
        private GradeBookRepositoryDb _repositoryDb;
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

            _repositoryDb = fixture.Create<GradeBookRepositoryDb>();
        }

        //[Test]
        //public async Task Login_Should_ReturnProjects()
        //{
        //    var teacherName = fixture.Create<string>();
        //    var userName = fixture.Create<string>();
        //    var classCode = fixture.Create<string>();
        //    using (var db = fixture.Create<Func<GradeBookDbContext>>()())
        //    {
        //        var teacher = new Person() { Name = teacherName };
        //        db.People.Add(new Person() { IsStudent = true, Name = userName });
        //        db.People.Add(teacher);
        //        db.Classes.Add(new Class() { Teacher = teacher, Id = classCode });
        //        db.SaveChanges();
        //    }

        //    var projects = await _repositoryDb.StudentLogin(userName, classCode);
        //    projects.Data.Should().BeEmpty();
        //}



        [Test]
        public async Task CreateTeacher()
        {
            var name = "cevans";
            var result = await _repositoryDb.CreateTeacher(new Person() { Name = name });

            result.Name.Should().Be(name);
        }

        [Test]
        public async Task AddClass()
        {
            var name = "cevans";
            var result = await _repositoryDb.CreateTeacher(new Person() { Name = name });

            var newClass = await _repositoryDb.CreateClass(new Class() { TeacherId = result.Id });
            newClass.Id.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task AddProject()
        {
            var name = "cevans";
            var result = await _repositoryDb.CreateTeacher(new Person() { Name = name });

            var newClass = await _repositoryDb.CreateClass(new Class() { TeacherId = result.Id });

            var project = await _repositoryDb.CreateProject(new CodeProject()
            {
                Name = "test",
                CsvCases = "10\r\n12",
                CsvExpectedOutput = "11\r\n13.2",
                Description = "Create an app that takes one parameter: the price of the item.  Then, add a 10% tax to the item and display the final value",
                DueDate = DateTimeOffset.Now.AddDays(1),
            });

            project.Id.Should().NotBe(0);
        }

        [Test]
        public async Task AddSubmission()
        {
            var name = "cevans";
            var result = await _repositoryDb.CreateTeacher(new Person() { Name = name });

            var newClass = await _repositoryDb.CreateClass(new Class() { TeacherId = result.Id });

            var project = await _repositoryDb.CreateProject(new CodeProject()
            {
                Name = "test",
                CsvCases = "10\r\n12",
                CsvExpectedOutput = "11\r\n13.2",
                Description = "Create an app that takes one parameter: the price of the item.  Then, add a 10% tax to the item and display the final value",
                DueDate = DateTimeOffset.Now.AddDays(1),
            });

            var student = await _repositoryDb.CreateStudent(new Person() { IsStudent = true, Name = "student" });

            var submission = await _repositoryDb.CreateSubmission(new Submission()
            {
                ProjectId = project.Id,
                //StudentId = student.Id
            });

            submission.Id.Should().NotBe(0);
        }

        //[Test]
        //public async Task TeacherLogin_Should_ReturnProjects()
        //{
        //    var name = "cevans";

        //    var result = await _repositoryDb.CreateTeacher(new Person() { Name = name });
        //    var newClass = await _repositoryDb.CreateClass(new Class() { TeacherId = result.Id });
        //    var project = await _repositoryDb.CreateProject(new CodeProject()
        //    {
        //        Name = "test",
        //        CsvCases = "10\r\n12",
        //        CsvExpectedOutput = "11\r\n13.2",
        //        Description = "Create an app that takes one parameter: the price of the item.  Then, add a 10% tax to the item and display the final value",
        //        DueDate = DateTimeOffset.Now.AddDays(1),
        //    });

        //    var loginResult = await _repositoryDb.TeacherLogin(name, newClass.Id);

        //    loginResult.Data.Should().HaveCount(1);
        //}

        [Test]
        public async Task EnrollInClass()
        {
            var name = "cevans";

            var result = await _repositoryDb.CreateTeacher(new Person() { Name = name });
            var student = await _repositoryDb.CreateStudent(new Person() { IsStudent = true, Name = "student" });

            var newClass = await _repositoryDb.CreateClass(new Class() { TeacherId = result.Id });

            var enrollment = await _repositoryDb.CreateEnrollment(student.Id, newClass.Id);
        }


        //[Test]
        //public async Task Login_NoStudent_Should_ThrowException()
        //{
        //    var result = await _repositoryDb.StudentLogin(fixture.Create<string>(), fixture.Create<string>());

        //    result.Status.Should().Be(RepositoryStatus.MissingUser);
        //}
    }
}