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

        public HomeViewModel(IInputBoxService inputBoxService)
        {
            _inputBoxService = inputBoxService;
            CreateSubmissionCommand = new DelegateCommand(CreateSubmission);
        }

        private async void CreateSubmission()
        {
            var code = await _inputBoxService.Show("Submission Code", "Please enter the code given by the teacher:");
            if (string.IsNullOrWhiteSpace(code) != true)
            {

            }
        }

        public DelegateCommand CreateSubmissionCommand { get; set; }
    }
}