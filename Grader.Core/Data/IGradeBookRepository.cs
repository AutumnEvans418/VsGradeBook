using System.Collections.Generic;
using System.Threading.Tasks;

namespace Grader
{
    public interface IGradeBookRepository
    {
        Task<int> GetPersonId(string userName);
        Task<RepositoryResult<IEnumerable<StudentProjectSummaryDto>>> StudentLogin(string userName, string classCode);
        Task<RepositoryResult<IEnumerable<CodeProject>>> TeacherLogin(string userName, string classId);
        Task<IEnumerable<Class>> GetClasses(int personId);
    }
}