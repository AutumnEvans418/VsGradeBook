using System;
using System.Threading.Tasks;
using Grader;

namespace AsyncToolWindowSample.ToolWindows
{
    public class ProjectPublishedViewModel : BindableViewModel
    {
        private readonly INavigationService _navigationService;
        private string _studentCode;
        private string _teacherCode;

        public string StudentCode
        {
            get => _studentCode;
            set => SetProperty(ref _studentCode,value);
        }

        public string TeacherCode
        {
            get => _teacherCode;
            set => SetProperty(ref _teacherCode,value);
        }

        public DelegateCommand DoneCommand { get; set; }
        public ProjectPublishedViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            DoneCommand = new DelegateCommand(Done);
        }

        public async override Task InitializeAsync(INavigationParameter parameter)
        {
            if (parameter["Project"] is CodeProject codeProject)
            {
                StudentCode = codeProject.StudentCode.ToString();
                TeacherCode = codeProject.TeacherCode.ToString();
            } 
        }

        private async void Done()
        {
            await _navigationService.ToPage("HomeView");
        }
    }
}