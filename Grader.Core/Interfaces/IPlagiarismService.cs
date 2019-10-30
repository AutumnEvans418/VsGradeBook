using System.Threading.Tasks;

namespace Grader.Web
{
    public interface IPlagiarismService
    {
        Task Check(int projectId);
    }
}