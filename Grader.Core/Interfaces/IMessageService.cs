using System;
using System.Threading.Tasks;

namespace AsyncToolWindowSample.ToolWindows
{
    public interface IMessageService
    {
        Task<string> ShowInputBox(string title, string msg, Func<string> importFunc);
        Task ShowAlert(string msg);
        Task ShowSaveDialog(string content);
        string ShowOpenDialog();
    }
}