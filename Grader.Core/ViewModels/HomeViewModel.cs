using System;
using System.Threading.Tasks;
using Grader;

namespace AsyncToolWindowSample.ToolWindows
{
    public interface IMessageService
    {
        Task<string> ShowInputBox(string title = null, string msg = null);
        Task ShowAlert(string msg);
    }


    public class HomeViewModel : BindableViewModel
    {
        private readonly IMessageService _inputBoxService;
        private readonly INavigationService _navigationService;
        private readonly IGradeBookRepository _gradeBookRepository;

        public HomeViewModel(IMessageService inputBoxService, INavigationService navigationService, IGradeBookRepository gradeBookRepository)
        {
            _inputBoxService = inputBoxService;
            _navigationService = navigationService;
            _gradeBookRepository = gradeBookRepository;
            CreateSubmissionCommand = new DelegateCommand(CreateSubmission);
            NewProjectCommand = new DelegateCommand(NewProject);
            SubmissionsCommand = new DelegateCommand(Submissions);
        }

        private async void Submissions()
        {
            var code = await _inputBoxService.ShowInputBox("Submission Code", "Please enter the teacher code:");
            if (string.IsNullOrWhiteSpace(code) != true)
            {
                var result = await _gradeBookRepository.GetSubmissions(Guid.Parse(code));
                if (result != null)
                {
                    await _navigationService.ToPage("SubmissionsView", new NavigationParameter(){{"Submissions", result}});
                }
                else
                {
                    await _inputBoxService.ShowAlert("Could not find");
                }
            }
            else
            {
                await _inputBoxService.ShowAlert("Could not find");
            }
        }

        private async void NewProject()
        {
            await _navigationService.ToPage("ProjectView");
        }

        private async void CreateSubmission()
        {
            var code = await _inputBoxService.ShowInputBox("Submission Code", "Please enter the code given by the teacher:");
            if (string.IsNullOrWhiteSpace(code) != true)
            {
                var result = await _gradeBookRepository.GetCodeProject(Guid.Parse(code));
                if (result != null)
                {
                    await _navigationService.ToPage("ProjectView", new NavigationParameter() { { "Project", result } });
                }
                else
                {
                    await _inputBoxService.ShowAlert("Could not find");
                }
            }
            else
            {
                await _inputBoxService.ShowAlert("Could not find");
            }
        }

        public DelegateCommand SubmissionsCommand { get; set; }
        public DelegateCommand NewProjectCommand { get; set; }
        public DelegateCommand CreateSubmissionCommand { get; set; }
    }
}