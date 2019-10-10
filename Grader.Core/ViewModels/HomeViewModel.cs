using System;
using Grader;
using Console = System.Console;

namespace AsyncToolWindowSample.ToolWindows
{
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
            try
            {
                var code = await _inputBoxService.ShowInputBox("Submission Code", "Please enter the teacher code:");
                if (string.IsNullOrWhiteSpace(code) != true)
                {
                    var result = await _gradeBookRepository.GetCodeProject(teacherCode: Guid.Parse(code));
                    if (result != null)
                    {
                        await _navigationService.ToPage("SubmissionsView", new NavigationParameter() { { "Project", result } });
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
            catch (Exception e)
            {
                Console.WriteLine(e);
                await _inputBoxService.ShowAlert(e.ToString());
            }
          
        }

        private async void NewProject()
        {
            await _navigationService.ToPage("ProjectView");
        }

        private async void CreateSubmission()
        {
            try
            {
                var code = await _inputBoxService.ShowInputBox("Submission Code", "Please enter the code given by the teacher:");
                if (string.IsNullOrWhiteSpace(code) != true)
                {
                    var result = await _gradeBookRepository.GetCodeProject(Guid.Parse(code));
                    if (result != null)
                    {
                        await _navigationService.ToPage("SubmissionProjectView", new NavigationParameter() { { "Project", result } });
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
            catch (Exception e)
            {
                Console.WriteLine(e);
                await _inputBoxService.ShowAlert(e.ToString());
            }

        }

        public DelegateCommand SubmissionsCommand { get; set; }
        public DelegateCommand NewProjectCommand { get; set; }
        public DelegateCommand CreateSubmissionCommand { get; set; }
    }
}