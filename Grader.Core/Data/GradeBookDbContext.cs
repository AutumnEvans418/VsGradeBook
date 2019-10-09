using Microsoft.EntityFrameworkCore;

namespace Grader
{
    public class GradeBookDbContext : DbContext
    {
        public DbSet<SubmissionFile> SubmissionFiles { get; set; }
        public DbSet<CodeProject> CodeProjects { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public GradeBookDbContext(DbContextOptions options): base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
         
            modelBuilder.Entity<Submission>(e =>
            {
                e.HasOne<CodeProject>().WithMany().HasForeignKey(p=>p.ProjectId);
                e.Property(p => p.StudentName).IsRequired();
            });

            modelBuilder.Entity<CodeProject>(e =>
            {
                e.Property(p => p.StudentCode).IsRequired();
                e.Property(p => p.TeacherCode).IsRequired();
                e.Property(p => p.DateCreated).IsRequired();
                e.Property(p => p.Description).IsRequired();
                e.Property(p => p.Name).IsRequired();
            });
            modelBuilder.Entity<SubmissionFile>(e =>
            {
                e.Property(p => p.Content).IsRequired();
                e.Property(p => p.FileName).IsRequired();
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}