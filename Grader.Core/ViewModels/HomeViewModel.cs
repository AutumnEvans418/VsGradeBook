using System;
using System.Threading.Tasks;
using Grader;
using Newtonsoft.Json;
using Console = System.Console;

namespace AsyncToolWindowSample.ToolWindows
{
    public class HomeViewModel : BindableViewModel
    {
        private readonly IMessageService _inputBoxService;
        private readonly INavigationService _navigationService;
        private readonly IGradeBookRepository _gradeBookRepository;

        public HomeViewModel(IMessageService inputBoxService, INavigationService navigationService, IGradeBookRepository gradeBookRepository) : base(navigationService)
        {
            _inputBoxService = inputBoxService;
            _navigationService = navigationService;
            _gradeBookRepository = gradeBookRepository;
            CreateSubmissionCommand = new DelegateCommandAsync(CreateSubmission);
            NewProjectCommand = new DelegateCommandAsync(NewProject);
            SubmissionsCommand = new DelegateCommandAsync(Submissions);
        }

        private async Task Submissions()
        {
            try
            {
                var code = await _inputBoxService.ShowInputBox("Submission Code", "Please enter the teacher code:",
                    ()=> GetProjectFile(true));
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

        private string GetProjectFile(bool teacherCode)
        {
            try
            {
                var file = _inputBoxService.ShowOpenDialog();
                if (file != null)
                {
                    var project = JsonConvert.DeserializeObject<CodeProject>(file);

                    if (teacherCode)
                    {
                        return project.TeacherCode.ToString();
                    }
                    else
                    {
                        return project.StudentCode.ToString();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _inputBoxService.ShowAlert("Invalid Project File!");
            }
            return null;
        }

        private async Task NewProject()
        {
            await _navigationService.ToPage("ProjectView");
        }

        private async Task CreateSubmission()
        {
            try
            {
                var code = await _inputBoxService.ShowInputBox("Submission Code", "Please enter the code given by the teacher:", 
                    () => GetProjectFile(false));
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