using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                var person = db.People.FirstOrDefault(p => p.IsStudent && p.Name == userName);

                var projects = db.CodeProjects.Where(p => p.ClassId == classCode);
                var submissions = db.Submissions.Where(p => p.StudentId == person.Id);
            }
        }
    }
}