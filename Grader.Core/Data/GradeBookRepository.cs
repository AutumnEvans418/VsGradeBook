using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Grader
{
    public enum RepositoryStatus
    {
        Success,
        MissingUser
    }
    public class RepositoryResult<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public RepositoryStatus Status { get; set; }
    }

    public class GradeBookRepository : IGradeBookRepository
    {
        private readonly Func<GradeBookDbContext> _dbFunc;

        public GradeBookRepository(Func<GradeBookDbContext> dbFunc)
        {
            _dbFunc = dbFunc;
        }

        public async Task<RepositoryResult<IEnumerable<StudentProjectDto>>> StudentLogin(string userName,string classCode)
        {
            using (var db = _dbFunc())
            {
                var person = await db.People.FirstOrDefaultAsync(p => p.IsStudent && p.Name == userName);
                if (person == null)
                {
                    return new RepositoryResult<IEnumerable<StudentProjectDto>>()
                    {
                        Message = $"Student with userName '{userName}' does not exist",
                        Status = RepositoryStatus.MissingUser,
                        Data = null
                    };
                }

                var projects = db.CodeProjects.Where(p => p.ClassId == classCode);
                var submissions = db.Submissions.Where(p => p.StudentId == person.Id);

                var studentProjects = projects
                    .GroupJoin(submissions, p => p.Id, p => p.ProjectId,
                        (project, enumerable) => new { project, submissions = enumerable });
                var data = studentProjects.Select(p => new StudentProjectDto()
                {
                    Name = p.project.Name,
                    Id = p.project.Id,
                    DueDate = p.project.DueDate,
                    IsBeingGraded = p.submissions.FirstOrDefault().IsSubmitted,
                    DateGraded = p.submissions.FirstOrDefault().DateGraded,
                    Status = StudentProjectStatus.Todo,
                    DateSubmitted = p.submissions.FirstOrDefault().SubmissionDate
                }).ToList();
                return new RepositoryResult<IEnumerable<StudentProjectDto>>()
                {
                    Data = data,
                };
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

        public Task<Person> CreateTeacher(Person person)
        {
            return Add(person);
        }

        public Task<Class> CreateClass(Class classs)
        {
            return Add(classs);
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

        public Task<Person> CreateStudent(Person person)
        {
            return Add(person);
        }

        public Task<Enrollment> CreateEnrollment(int studentId, string newClassId)
        {
            var enroll = new Enrollment() { ClassId = newClassId, StudentId = studentId };
            return Add(enroll);
        }

        public async Task<RepositoryResult<IEnumerable<CodeProject>>> TeacherLogin(string userName, string classId)
        {
            using (var db = _dbFunc())
            {
                var result = new RepositoryResult<IEnumerable<CodeProject>>();
                var person = await db.People.FirstOrDefaultAsync(p => p.IsStudent != true && p.Name == userName);
                if (person == null)
                {
                    result.Message = $"Teacher with userName '{userName}' does not exist";
                    result.Status = RepositoryStatus.MissingUser;
                    result.Data = null;
                    return result;
                }

                var data = db.CodeProjects.Include(p=>p.Submissions).Where(p => p.ClassId == classId).ToList();
                result.Data = data;
                return result;
            }
        }
    }

    public class ProjectSubmissionDto
    {
        public CodeProject CodeProject { get; set; }
        public Submission Submission { get; set; }
    }
}