using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Grader;

namespace AsyncToolWindowSample.ToolWindows
{
    public class SubmissionsViewModel : BindableViewModel
    {
        private Submission _selectedSubmission;
        private ObservableCollection<Submission> _submissions;

        public Submission SelectedSubmission
        {
            get => _selectedSubmission;
            set => SetProperty(ref _selectedSubmission, value);
        }

        public ObservableCollection<Submission> Submissions
        {
            get => _submissions;
            set => SetProperty(ref _submissions, value);
        }

        public async override Task InitializeAsync(INavigationParameter parameter)
        {
        }

        public SubmissionsViewModel()
        {
            Submissions = new ObservableCollection<Submission>();
        }
    }
    public class ProjectsViewModel : BindableViewModel
    {
        private readonly IGradeBookRepository _repository;
        private readonly INavigationService _navigationService;
        private CodeProject _selectedProject;
        private ObservableCollection<CodeProject> _projects;

        public CodeProject SelectedProject
        {
            get => _selectedProject;
            set => SetProperty(ref _selectedProject, value);
        }

        public ObservableCollection<CodeProject> Projects
        {
            get => _projects;
            set => SetProperty(ref _projects, value);
        }

        private Class _class;
        public override async Task InitializeAsync(INavigationParameter parameter)
        {
            if (parameter["Class"] is Class cls)
            {
                _class = cls;
                await Refresh();
            }
        }

        private async Task Refresh()
        {
            Projects = new ObservableCollection<CodeProject>(await _repository.GetCodeProjects(_class.Id));
        }

        public DelegateCommand AddCommand { get; set; }
        public DelegateCommand OpenCommand { get; set; }
        public DelegateCommand SubmissionsCommand { get; set; }
        public ProjectsViewModel(IGradeBookRepository repository, INavigationService navigationService)
        {
            _repository = repository;
            _navigationService = navigationService;
            Projects = new ObservableCollection<CodeProject>();
            AddCommand = new DelegateCommand(Add);
            OpenCommand = new DelegateCommand(Open);
            SubmissionsCommand = new DelegateCommand(Submissions);
        }

        private async void Submissions()
        {
            await _navigationService.ToPage("SubmissionsView", new NavigationParameter() { { "Project", SelectedProject } });
        }

        private async void Open()
        {
            await _navigationService.ToPage("ProjectView", new NavigationParameter() { { "Project", SelectedProject } });
            await Refresh();

        }

        private async void Add()
        {
            await _navigationService.ToPage("ProjectView", new NavigationParameter() { { "Class", _class } });
            await Refresh();
        }
    }
}