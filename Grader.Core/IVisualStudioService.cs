using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsyncToolWindowSample.Models
{
    public interface IVisualStudioService
    {
        Task<IEnumerable<string>> GetCSharpFilesAsync();
        void OpenOrCreateCSharpFile(string fileName, string content);
    }
}