using System.Threading.Tasks;
using Grader;

namespace AsyncToolWindowSample.ToolWindows
{
    public class CreateAccountViewModel : BindableViewModel
    {
        private readonly IToolWindow _toolWindow;
        private readonly IGradeBookRepository _gradeBookRepository;
        public Person Person { get; set; }

        public CreateAccountViewModel(IToolWindow toolWindow, IGradeBookRepository gradeBookRepository)
        {
            _toolWindow = toolWindow;
            _gradeBookRepository = gradeBookRepository;
            Person = new Person();
             CreateAccountCommand = new DelegateCommandAsync(CreateAccount);
             BackCommand = new DelegateCommandAsync(Back);
        }

        public override void Initialize(INavigationParameter parameter)
        {
            if (parameter["IsStudent"] is bool isStudent)
            {
                Person.IsStudent = isStudent;
            }
            base.Initialize(parameter);
        }

        private async Task Back()
        {
            await _toolWindow.ToPage("LoginView");
        }

        private async Task CreateAccount()
        {
            await _toolWindow.ToPage("StudentHomeView", new NavigationParameter());
        }


        public DelegateCommandAsync CreateAccountCommand { get; set; }
        public DelegateCommandAsync BackCommand { get; set; }
    }
}