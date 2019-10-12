using System.Threading.Tasks;

namespace AsyncToolWindowSample.ToolWindows
{
    public class BindableViewModel : BindableBase, INavigationAware, INavigationAwareAsync
    {
        public virtual void Initialize(INavigationParameter parameter)
        {
        }

        public virtual async Task InitializeAsync(INavigationParameter parameter)
        {
        }
    }
}