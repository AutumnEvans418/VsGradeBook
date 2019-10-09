using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using AsyncToolWindowSample.Models;
using Grader;

namespace AsyncToolWindowSample.ToolWindows
{
    public class SubmissionsViewModel : BindableViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IVisualStudioService _visualStudioService;
        private readonly IGradeBookRepository _gradeBookRepository;
        private Submission _selectedSubmission;
        private ObservableCollection<Submission> _submissions;
        private CodeProject _codeProject;

        public Submission SelectedSubmission
        {
            get => _selectedSubmission;
            set => SetProperty(ref _selectedSubmission, value, SelectedSubmissionChanged);
        }

        private void SelectedSubmissionChanged()
        {
            if (SelectedSubmission != null)
            {
                var name = SelectedSubmission.StudentName;
                foreach (var selectedSubmissionSubmissionFile in SelectedSubmission.SubmissionFiles)
                {
                    var fileName = name + Path.GetFileName(selectedSubmissionSubmissionFile.FileName);
                    _visualStudioService.OpenOrCreateCSharpFile(fileName, selectedSubmissionSubmissionFile.Content);
                }

            }
        }

        public CodeProject CodeProject
        {
            get => _codeProject;
            set => SetProperty(ref _codeProject, value);
        }

        public ObservableCollection<Submission> Submissions
        {
            get => _submissions;
            set => SetProperty(ref _submissions, value);
        }

        public DelegateCommand DoneCommand { get; }

        public override async Task InitializeAsync(INavigationParameter parameter)
        {
            if (parameter["Project"] is CodeProject project)
            {
                CodeProject = project;
                Submissions = new ObservableCollection<Submission>(await _gradeBookRepository.GetSubmissions(project.TeacherCode));
            }
        }

        public SubmissionsViewModel(INavigationService navigationService, IVisualStudioService visualStudioService, IGradeBookRepository gradeBookRepository)
        {
            _navigationService = navigationService;
            _visualStudioService = visualStudioService;
            _gradeBookRepository = gradeBookRepository;
            Submissions = new ObservableCollection<Submission>();
            DoneCommand = new DelegateCommand(Done);
        }

        private async void Done()
        {
            await _navigationService.ToPage("HomeView");
        }
    }
}