using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Grader
{
    public interface IGradeBookRepository
    {
        Task<CodeProject> GetCodeProject(Guid? studentCode = null, Guid? teacherCode = null);
        Task<CodeProject> AddProject(CodeProject project);
        Task<Submission> AddSubmission(Submission submission);
        Task<IEnumerable<Submission>> GetSubmissions(Guid teacherCode);
        Task<IEnumerable<CodeProject>> GetCodeProjects();
    }
}