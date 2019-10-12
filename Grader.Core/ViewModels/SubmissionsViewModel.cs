using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using AsyncToolWindowSample.Models;
using Grader;
using Console = System.Console;

namespace AsyncToolWindowSample.ToolWindows
{
    public class SubmissionsViewModel : BindableViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IVisualStudioService _visualStudioService;
        private readonly IGradeBookRepository _gradeBookRepository;
        private readonly IMessageService _messageService;
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
                    var fileName = name + "-" + Path.GetFileName(selectedSubmissionSubmissionFile.FileName);
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
            try
            {
                if (parameter["Project"] is CodeProject project)
                {
                    CodeProject = project;
                    Submissions = new ObservableCollection<Submission>(await _gradeBookRepository.GetSubmissions(project.TeacherCode));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await _messageService.ShowAlert(e.ToString());
            }
           
        }

        public SubmissionsViewModel(INavigationService navigationService,
            IVisualStudioService visualStudioService, IGradeBookRepository gradeBookRepository, IMessageService messageService) : base(navigationService)
        {
            _navigationService = navigationService;
            _visualStudioService = visualStudioService;
            _gradeBookRepository = gradeBookRepository;
            _messageService = messageService;
            Submissions = new ObservableCollection<Submission>();
            DoneCommand = new DelegateCommandAsync(Done);
        }

        private async Task Done()
        {
            await _navigationService.ToPage("HomeView");
        }
    }
}