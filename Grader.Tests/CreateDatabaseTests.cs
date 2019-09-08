using System;
using System.IO;
using NUnit.Framework;

namespace Grader.Tests
{
    [TestFixture]
    public class CreateDatabaseTests
    {
        private CreateDatabase db;
        [SetUp]
        public void Setup()
        {
            db = new CreateDatabase();
            var file = db.GradeBookPath;
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }


        [Test]
        public void CreateDb()
        {
            db.Initialize();
            var result = db.GetGradeBookDbContext();

            result.People.Add(new Person());
            result.SaveChanges();
            result.Dispose();
        }
    }
}