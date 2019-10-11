using System.Threading.Tasks;

namespace AsyncToolWindowSample.ToolWindows
{
    public interface IMessageService
    {
        Task<string> ShowInputBox(string title = null, string msg = null);
        Task ShowAlert(string msg);
        Task ShowSaveDialog(string content);
    }
}