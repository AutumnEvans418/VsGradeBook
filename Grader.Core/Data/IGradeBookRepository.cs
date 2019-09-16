using System.Collections.Generic;
using System.Threading.Tasks;

namespace Grader
{
    public interface IGradeBookRepository
    {
        Task<RepositoryResult<IEnumerable<StudentProjectDto>>> StudentLogin(string userName, string classCode);
        Task<RepositoryResult<IEnumerable<CodeProject>>> TeacherLogin(string userName, string classId);
    }
}