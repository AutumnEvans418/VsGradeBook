using System.Threading.Tasks;

namespace AsyncToolWindowSample.ToolWindows
{
    public interface IToolWindow
    {
        Task ToPage(string page);
        Task ToPage(string page, INavigationParameter parameter);
    }
}