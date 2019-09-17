using System;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace Grader
{
    public class CreateDatabase
    {

        public const string GradeBookDb = "gradebook.db";
        public const string VsGradeBook = "VsGradeBook";
        public string GradeBookPath = 
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), VsGradeBook, GradeBookDb);
        private DbContextOptions options;
        public void Initialize()
        {

            var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            var vsGrader = "VsGradeBook";

            var directory = Path.Combine(path, vsGrader);

            if (Directory.Exists(directory) != true)
            {
                Directory.CreateDirectory(directory);
            }

            var gradeBookDb = "gradebook.db";

            var file = Path.Combine(directory, gradeBookDb);

            var builder = new DbContextOptionsBuilder().UseSqlite($"Data Source={file}").EnableSensitiveDataLogging();
            
            options = builder.Options;

            using (var db = GetGradeBookDbContext())
            {
                db.Database.EnsureCreated();
            }
        }
        public GradeBookDbContext GetGradeBookDbContext()
        {
            return new GradeBookDbContext(options);
        }
    }
}