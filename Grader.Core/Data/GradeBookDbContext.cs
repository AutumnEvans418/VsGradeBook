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
    public class GradeBookDbContext : DbContext
    {
        public DbSet<Person> People { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<CodeProject> CodeProjects { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public GradeBookDbContext(DbContextOptions options): base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);
        }
    }
}