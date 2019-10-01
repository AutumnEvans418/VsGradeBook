using System;

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
            StudentCode = Guid.NewGuid().ToString();
            TeacherCode = Guid.NewGuid().ToString();
            DoneCommand = new DelegateCommand(Done);
        }

        private async void Done()
        {
            await _navigationService.ToPage("HomeView");
        }
    }
}