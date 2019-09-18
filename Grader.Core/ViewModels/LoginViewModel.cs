using System.Threading.Tasks;
using Grader;

namespace AsyncToolWindowSample.ToolWindows
{
    public class LoginViewModel : BindableViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IGradeBookRepository _repository;

        public LoginViewModel(INavigationService navigationService, IGradeBookRepository repository)
        {
            _navigationService = navigationService;
            _repository = repository;
            LoginCommand = new DelegateCommandAsync(Login);
            CreateAccountCommand = new DelegateCommandAsync(CreateAccount);
        }

        private async Task CreateAccount()
        {
            await _navigationService.ToPage("CreateAccountView", new NavigationParameter(){{"IsStudent", IsStudent}});
        }

        private async Task Login()
        {

            var id = await _repository.GetPersonId(Name);

            await _navigationService.ToPage("ClassesView", new NavigationParameter() {{"PersonId", id}});
            //if (IsStudent)
            //{
            //    var project = await _repository.StudentLogin(Name, CourseCode);
            //    if (project.Status == RepositoryStatus.Success)
            //    {
            //        await _navigationService.ToPage("StudentHomeView", new NavigationParameter() { { "Projects", project.Data } });
            //    }
            //    else
            //    {
            //        ErrorMessage = project.Message;
            //    }
            //}
            //else
            //{
            //    var project = await _repository.TeacherLogin(Name, CourseCode);
            //    if (project.Status == RepositoryStatus.Success)
            //    {
            //        await _navigationService.ToPage("TeacherHomeView", new NavigationParameter() { { "Projects", project.Data } });
            //    }
            //    else
            //    {
            //        ErrorMessage = project.Message;
            //    }
            //}

        }

        public string ErrorMessage { get; set; }
        public bool IsStudent { get; set; }
        public string Name { get; set; }
        public string CourseCode { get; set; }
        public DelegateCommandAsync LoginCommand { get; set; }
        public DelegateCommandAsync CreateAccountCommand { get; set; }
    }
}