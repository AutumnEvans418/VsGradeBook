using System.Threading.Tasks;

namespace AsyncToolWindowSample.ToolWindows
{
    public interface INavigationAwareAsync
    {
        Task InitializeAsync(INavigationParameter parameter);
    }
}