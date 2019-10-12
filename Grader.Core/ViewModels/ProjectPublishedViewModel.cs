using System;
using System.Threading.Tasks;
using Grader;
using Newtonsoft.Json;

namespace AsyncToolWindowSample.ToolWindows
{
    public class ProjectPublishedViewModel : BindableViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IMessageService _messageService;
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
        public DelegateCommand SaveCommand { get; set; }
        public ProjectPublishedViewModel(INavigationService navigationService, IMessageService messageService) : base(navigationService)
        {
            _navigationService = navigationService;
            _messageService = messageService;
            DoneCommand = new DelegateCommandAsync(Done);
            SaveCommand = new DelegateCommand(Save);
        }

        private void Save()
        {
            var json = JsonConvert.SerializeObject(_codeProject);
            _messageService.ShowSaveDialog(json);
        }

        private CodeProject _codeProject;
        public async override Task InitializeAsync(INavigationParameter parameter)
        {
            if (parameter["Project"] is CodeProject codeProject)
            {
                _codeProject = codeProject;
                StudentCode = codeProject.StudentCode.ToString();
                TeacherCode = codeProject.TeacherCode.ToString();
            } 
        }

        private async Task Done()
        {
            await _navigationService.ToPage("HomeView");
        }
    }
}