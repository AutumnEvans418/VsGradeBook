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

        public async Task<int> GetPersonId(string userName)
        {
            //using (var db = _dbFunc())
            //{
            //    var id = await db.People.FirstOrDefaultAsync(p => p.Name == userName);
            //    if (id == null) throw new Exception($"Could not find user with userName '{userName}'");
            //    return id.Id;
            //}
            return 0;
        }

        public Task<Person> AddPerson(Person person)
        {
            return Add(person);
        }

        public async Task<Person> UpdatePerson(Person person)
        {
            using (var db = _dbFunc())
            {
               // db.People.Update(person);
                await db.SaveChangesAsync();
                return person;
            }
        }

        public async Task DeletePerson(int personId)
        {
            using (var db = _dbFunc())
            {
               // var person = await db.People.FirstOrDefaultAsync(p => p.Id == personId);
               // db.People.Remove(person);
                await db.SaveChangesAsync();
            }
        }

        //public async Task<RepositoryResult<IEnumerable<StudentProjectSummaryDto>>> StudentLogin(string userName, string classCode)
        //{
        //    using (var db = _dbFunc())
        //    {
        //        var person = await db.People.FirstOrDefaultAsync(p => p.IsStudent && p.Name == userName);
        //        if (person == null)
        //        {
        //            return new RepositoryResult<IEnumerable<StudentProjectSummaryDto>>()
        //            {
        //                Message = $"Student with userName '{userName}' does not exist",
        //                Status = RepositoryStatus.MissingUser,
        //                Data = null
        //            };
        //        }

        //        var projects = db.CodeProjects.Where(p => p.ClassId == classCode);
        //        var submissions = db.Submissions.Where(p => p.StudentId == person.Id);

        //        var studentProjects = projects
        //            .GroupJoin(submissions, p => p.Id, p => p.ProjectId,
        //                (project, enumerable) => new { project, submissions = enumerable });
        //        var data = studentProjects.Select(p => new StudentProjectSummaryDto()
        //        {
        //            Name = p.project.Name,
        //            Id = p.project.Id,
        //            DueDate = p.project.DueDate,
        //            IsBeingGraded = p.submissions.FirstOrDefault().IsSubmitted,
        //            DateGraded = p.submissions.FirstOrDefault().DateGraded,
        //            DateSubmitted = p.submissions.FirstOrDefault().SubmissionDate,
        //            SubmissionPublished = p.submissions.FirstOrDefault().IsSubmitted,
        //            HasSubmission = p.submissions.Any()
        //        }).ToList();
        //        return new RepositoryResult<IEnumerable<StudentProjectSummaryDto>>()
        //        {
        //            Data = data,
        //        };
        //    }
        //}

        //public async Task<ProjectSubmissionDto> GetProject(int studentId, int projectId)
        //{
        //    using (var db = _dbFunc())
        //    {
        //        return
        //            await db.CodeProjects
        //                .Select(p => new ProjectSubmissionDto()
        //                {
        //                    CodeProject = p,
        //                    Submission = p.Submissions.FirstOrDefault()
        //                })
        //                .FirstOrDefaultAsync(p =>
        //                    p.CodeProject.Id == projectId &&
        //                    (p.Submission.StudentId == studentId || p.Submission == null));
        //    }
        //}

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

        //public async Task<RepositoryResult<IEnumerable<CodeProject>>> TeacherLogin(string userName, string classId)
        //{
        //    using (var db = _dbFunc())
        //    {
        //        var result = new RepositoryResult<IEnumerable<CodeProject>>();
        //        var person = await db.People.FirstOrDefaultAsync(p => p.IsStudent != true && p.Name == userName);
        //        if (person == null)
        //        {
        //            result.Message = $"Teacher with userName '{userName}' does not exist";
        //            result.Status = RepositoryStatus.MissingUser;
        //            result.Data = null;
        //            return result;
        //        }

        //        var data = db.CodeProjects.Include(p => p.Submissions).Where(p => p.ClassId == classId).ToList();
        //        result.Data = data;
        //        return result;
        //    }
        //}

        public async Task<IEnumerable<Class>> GetClasses(int personId)
        {
            using (var db = _dbFunc())
            {
                //var classes = await db.Classes
                //    .Where(p => p.TeacherId == personId || p.Enrollments.Any(r => r.StudentId == personId))
                //    .ToListAsync();
               // return classes;
               return null;
            }
        }

        public async Task<Class> UpdateClass(Class cClass)
        {
            using (var db = _dbFunc())
            {
               // db.Classes.Update(cClass);
                await db.SaveChangesAsync();
                return cClass;
            }
        }

        public async Task<Class> AddClass(Class cClass)
        {
            return await Add(cClass);
        }

        public async Task DeleteClass(string id)
        {
            using (var db = _dbFunc())
            {
              //  var classs = await db.Classes.FirstOrDefaultAsync(p => p.Id == id);
               // db.Classes.Remove(classs);
                await db.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Enrollment>> GetEnrollments()
        {
            using (var db = _dbFunc())
            {
               // return await db.Enrollments.ToListAsync();
               return null;
            }
        }

        public async Task<Enrollment> GetEnrollment(int enrollmentId)
        {
            using (var db = _dbFunc())
            {
                return null;

                // return await db.Enrollments.FirstOrDefaultAsync(p => p.Id == enrollmentId);
            }
        }

        public async Task<Enrollment> AddEnrollment(Enrollment enrollment)
        {
            return await Add(enrollment);
        }

        public async Task<Enrollment> UpdateEnrollment(Enrollment enroll)
        {
            using (var db = _dbFunc())
            {
              //  db.Enrollments.Update(enroll);
                await db.SaveChangesAsync();
                return enroll;
            }
        }

        public async Task DeleteEnrollment(int enrollmentId)
        {
            using (var db = _dbFunc())
            {
               // var enroll = await db.Enrollments.FirstOrDefaultAsync(p => p.Id == enrollmentId);
               // db.Enrollments.Remove(enroll);
                await db.SaveChangesAsync();
            }
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
    }


}