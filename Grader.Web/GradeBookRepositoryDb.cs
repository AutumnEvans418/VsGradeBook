using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Grader
{
    public class GradeBookRepositoryDb : IGradeBookRepository
    {
        private readonly Func<GradeBookDbContext> _dbFunc;

        public GradeBookRepositoryDb(Func<GradeBookDbContext> dbFunc)
        {
            _dbFunc = dbFunc;
        }
      

        public Task<CodeProject> CreateProject(CodeProject codeProject)
        {
            return Add(codeProject);
        }

        async Task<T> Add<T>(T item) where T : class
        {
            using (var db = _dbFunc())
            {
                db.Set<T>().Add(item);
                await db.SaveChangesAsync();
                return item;
            }
        }
        public Task<Submission> CreateSubmission(Submission submission)
        {
            return Add(submission);
        }

      
      
      

      

        public async Task<CodeProject> GetCodeProject(Guid? studentCode, Guid? teacherCode)
        {
            using (var db = _dbFunc())
            {
                return await db.CodeProjects.FirstOrDefaultAsync(p => p.StudentCode == studentCode || p.TeacherCode == teacherCode);
            }
        }

        public async Task<CodeProject> AddProject(CodeProject project)
        {
           return await Add(project);
        }

        public async Task<Submission> AddSubmission(Submission submission)
        {
            return await Add(submission);
        }

        public async Task<IEnumerable<Submission>> GetSubmissions(Guid teacherCode)
        {
            using (var db = _dbFunc())
            {
                var projects = db.CodeProjects.Where(p => p.TeacherCode == teacherCode).Select(p=>p.Id);
                return await db.Submissions.Where(p => projects.Contains(p.ProjectId)).Include(p=>p.SubmissionFiles).ToListAsync();
            }
        }

        public async Task<IEnumerable<CodeProject>> GetCodeProjects()
        {
            using (var db = _dbFunc())
            {
                return await db.CodeProjects.ToListAsync();
            }
        }

        public async Task<IEnumerable<Submission>> GetSubmissions(int projectId)
        {
            using (var db = _dbFunc())
            {
                return await db.Submissions.Where(p => p.ProjectId == projectId).Include(p => p.SubmissionFiles)
                    .ToListAsync();
            }
        }

        public async Task Plagiarized(IEnumerable<Submission> submissions)
        {
            using (var db = _dbFunc())
            {
                foreach (var submission in submissions)
                {
                    submission.IsPlagiarized = true;
                    db.Submissions.Update(submission);
                }
                await db.SaveChangesAsync();
            }
        }
    }


}