using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Grader
{
    public class GradeBookRepository : IGradeBookRepository
    {
        private readonly Func<GradeBookDbContext> _dbFunc;

        public GradeBookRepository(Func<GradeBookDbContext> dbFunc)
        {
            _dbFunc = dbFunc;
        }
        public async Task<IEnumerable<StudentProjectDto>> StudentLogin(string userName, string classCode)
        {
            using (var db = _dbFunc())
            {
                var person = await db.People.FirstOrDefaultAsync(p => p.IsStudent && p.Name == userName);

                var projects = db.CodeProjects.Where(p => p.ClassId == classCode);
                var submissions = db.Submissions.Where(p => p.StudentId == person.Id);

                var studentProjects = projects
                    .GroupJoin(submissions, p => p.Id, p => p.ProjectId,
                        (project, enumerable) => new {project, submissions = enumerable});
                return studentProjects.Select(p => new StudentProjectDto()
                {
                    Name = p.project.Name,
                    Id = p.project.Id,
                    DueDate = p.project.DueDate,
                    IsBeingGraded = p.submissions.FirstOrDefault().IsSubmitted,
                    DateGraded = p.submissions.FirstOrDefault().DateGraded,
                    Status = "",

                }).ToList();

            }
        }

        public async Task<ProjectSubmissionDto> GetProject(int studentId, int projectId)
        {
            using (var db = _dbFunc())
            {
                return
                    await db.CodeProjects
                        .Select(p => new ProjectSubmissionDto()
                        {
                            CodeProject = p,
                            Submission = p.Submissions.FirstOrDefault()
                        })
                        .FirstOrDefaultAsync(p =>
                            p.CodeProject.Id == projectId &&
                            (p.Submission.StudentId == studentId || p.Submission == null));
            }
        }
    }

    public class ProjectSubmissionDto
    {
        public CodeProject CodeProject { get; set; }
        public Submission Submission { get; set; }
    }
}