using System.Threading.Tasks;

namespace AsyncToolWindowSample.ToolWindows
{
    public interface INavigationService
    {
        Task ToPage(string page);
        Task ToPage(string page, INavigationParameter parameter);
        Task ToModalPage(string page, INavigationParameter parameter);
    }
}