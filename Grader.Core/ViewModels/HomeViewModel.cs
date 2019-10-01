using System.Threading.Tasks;

namespace AsyncToolWindowSample.ToolWindows
{
    public interface IInputBoxService
    {
        Task<string> Show(string title = null, string msg = null);
    }
    public class HomeViewModel : BindableViewModel
    {
        private readonly IInputBoxService _inputBoxService;
        private readonly INavigationService _navigationService;

        public HomeViewModel(IInputBoxService inputBoxService, INavigationService navigationService)
        {
            _inputBoxService = inputBoxService;
            _navigationService = navigationService;
            CreateSubmissionCommand = new DelegateCommand(CreateSubmission);
            NewProjectCommand = new DelegateCommand(NewProject);
            SubmissionsCommand = new DelegateCommand(Submissions);
        }

        private async void Submissions()
        {
            await _navigationService.ToPage("SubmissionsView");
        }

        private async void NewProject()
        {
            await _navigationService.ToPage("ProjectView");
        }

        private async void CreateSubmission()
        {
            var code = await _inputBoxService.Show("Submission Code", "Please enter the code given by the teacher:");
            if (string.IsNullOrWhiteSpace(code) != true)
            {

            }
        }

        public DelegateCommand SubmissionsCommand { get; set; }
        public DelegateCommand NewProjectCommand { get; set; }
        public DelegateCommand CreateSubmissionCommand { get; set; }
    }
}