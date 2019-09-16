using System.Threading.Tasks;
using Grader;

namespace AsyncToolWindowSample.ToolWindows
{
    public class LoginViewModel : BindableViewModel
    {
        private readonly IToolWindow _toolWindow;
        private readonly IGradeBookRepository _repository;

        public LoginViewModel(IToolWindow toolWindow, IGradeBookRepository repository)
        {
            _toolWindow = toolWindow;
            _repository = repository;
            LoginCommand = new DelegateCommandAsync(Login);
            CreateAccountCommand = new DelegateCommandAsync(CreateAccount);
        }

        private async Task CreateAccount()
        {
            await _toolWindow.ToPage("CreateAccountView", new NavigationParameter(){{"IsStudent", IsStudent}});
        }

        private async Task Login()
        {
            if (IsStudent)
            {
                var project = await _repository.StudentLogin(Name, CourseCode);
                if (project.Status == RepositoryStatus.Success)
                {
                    await _toolWindow.ToPage("StudentHomeView", new NavigationParameter() { { "Projects", project.Data } });
                }
                else
                {
                    ErrorMessage = project.Message;
                }
            }
            else
            {
                var project = await _repository.TeacherLogin(Name, CourseCode);
                if (project.Status == RepositoryStatus.Success)
                {
                    await _toolWindow.ToPage("TeacherHomeView", new NavigationParameter() { { "Projects", project.Data } });
                }
                else
                {
                    ErrorMessage = project.Message;
                }
            }

        }

        public string ErrorMessage { get; set; }
        public bool IsStudent { get; set; }
        public string Name { get; set; }
        public string CourseCode { get; set; }
        public DelegateCommandAsync LoginCommand { get; set; }
        public DelegateCommandAsync CreateAccountCommand { get; set; }
    }
}