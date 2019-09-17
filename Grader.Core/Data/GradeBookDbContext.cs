using Microsoft.EntityFrameworkCore;

namespace Grader
{
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