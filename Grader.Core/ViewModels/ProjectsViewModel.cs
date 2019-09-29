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
        private CodeProject _selectedProject;
        private ObservableCollection<CodeProject> _projects;

        public CodeProject SelectedProject
        {
            get => _selectedProject;
            set => SetProperty(ref _selectedProject,value);
        }

        public ObservableCollection<CodeProject> Projects
        {
            get => _projects;
            set => SetProperty(ref _projects,value);
        }

        private Class _class;
        public override async Task InitializeAsync(INavigationParameter parameter)
        {
            if (parameter["Class"] is Class cls)
            {
                _class = cls;
                Projects = new ObservableCollection<CodeProject>(await _repository.GetCodeProjects(cls.Id));
            }
        }

        public DelegateCommand AddCommand { get; set; }
        public DelegateCommand OpenCommand { get; set; }
        public DelegateCommand SubmissionsCommand { get; set; }
        public ProjectsViewModel(IGradeBookRepository repository)
        {
            _repository = repository;
            Projects = new ObservableCollection<CodeProject>();
            AddCommand = new DelegateCommand(Add);
            OpenCommand = new DelegateCommand(Open);
            SubmissionsCommand = new DelegateCommand(Submissions);
        }

        private void Submissions()
        {
            
        }

        private void Open()
        {
        }

        private void Add()
        {
            
        }
    }
}