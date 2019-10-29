using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsyncToolWindowSample.Models
{
    public class FileContent
    {
        public string FileName { get; set; }
        public string Content { get; set; }
    }
    public interface IVisualStudioService
    {
        Task<IEnumerable<FileContent>> GetCSharpFilesAsync();
        Task OpenOrCreateCSharpFile(string fileName, string content);
    }
}