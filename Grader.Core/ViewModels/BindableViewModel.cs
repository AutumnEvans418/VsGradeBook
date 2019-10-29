using System.Threading.Tasks;

namespace AsyncToolWindowSample.ToolWindows
{
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public class BindableViewModel : BindableBase, INavigationAware, INavigationAwareAsync
    {
        private readonly INavigationService _navigationService;

        public BindableViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            BackCommand = new DelegateCommand(Back);
        }

        private async void Back()
        {
            await _navigationService.ToPage("HomeView");
        }

        public DelegateCommand BackCommand { get; }

        public virtual void Initialize(INavigationParameter parameter)
        {
        }

        public virtual async Task InitializeAsync(INavigationParameter parameter)
        {
        }

    }
}