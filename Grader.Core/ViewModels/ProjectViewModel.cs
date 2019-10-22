using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AsyncToolWindowSample.Models;
using Grader;
using Console = System.Console;

namespace AsyncToolWindowSample.ToolWindows
{
    public class ProjectViewModel : BindableViewModel
    {
        private readonly IVisualStudioService _visualStudioService;
        private readonly IConsoleAppGrader _grader;
        private readonly INavigationService _navigationService;
        private readonly IGradeBookRepository _gradeBookRepository;
        private readonly IMessageService _messageService;
        private string _actualOutput;
        private string _errorMessage;
        private CodeProject _codeProject;
        private Submission _submission;
        private bool _isStudentSubmission;
        private ObservableCollection<IGradeCase> _cases;
        private bool _isShowingCode;
        private string _parseErrorMessage;

        public ProjectViewModel(IVisualStudioService visualStudioService,
            IConsoleAppGrader grader, INavigationService navigationService, IGradeBookRepository gradeBookRepository, IMessageService messageService) : base(navigationService)
        {
            Cases = new ObservableCollection<IGradeCase>();
            _visualStudioService = visualStudioService;
            _grader = grader;
            _navigationService = navigationService;
            _gradeBookRepository = gradeBookRepository;
            _messageService = messageService;
            CodeProject = new CodeProject { CsvCases = "", CsvExpectedOutput = "Hello World!" };
            TestCommand = new DelegateCommandAsync(Test);
            SubmitCommand = new DelegateCommandAsync(Submit);
            Submission = new Submission();
        }

        public string CsvCases
        {
            get => CodeProject.CsvCases;
            set
            {
                CodeProject.CsvCases = SetProperty(CodeProject.CsvCases, value);
                CodeChanged();
            }
        }

        public string CsvExpectedOutput
        {
            get => CodeProject.CsvExpectedOutput;
            set
            {
                CodeProject.CsvExpectedOutput = SetProperty(CodeProject.CsvExpectedOutput, value);
                CodeChanged();
            }
        }

        public string ParseErrorMessage
        {
            get => _parseErrorMessage;
            set => SetProperty(_parseErrorMessage,value);
        }

        async  void CodeChanged()
        {
            try
            {
                Cases = new ObservableCollection<IGradeCase>(ConvertTextToGradeCases());
                ParseErrorMessage = "";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ParseErrorMessage = e.Message;
            }
        }
        public bool IsStudentSubmission
        {
            get => _isStudentSubmission;
            set => SetProperty(ref _isStudentSubmission, value);
        }

        public override async Task InitializeAsync(INavigationParameter parameter)
        {
            Submission = new Submission();

            if (parameter.ContainsKey("Project") && parameter["Project"] is CodeProject project)
            {
                CodeProject = project;
                IsStudentSubmission = true;
                IsShowingCode = false;
                Submission.ProjectId = CodeProject.Id;
            }
            else
            {
                IsShowingCode = true;
                CodeProject = new CodeProject();
            }
            CodeChanged();
        }

        public CodeProject CodeProject
        {
            get => _codeProject;
            set => SetProperty(ref _codeProject, value);
        }

        private async Task Submit()
        {
            try
            {
                if (IsStudentSubmission)
                {
                    var codes = await _visualStudioService.GetCSharpFilesAsync();

                    Submission.SubmissionFiles = codes.Select(p => new SubmissionFile() { Content = p.Content, FileName = p.FileName }).ToList();
                    if (string.IsNullOrWhiteSpace(Submission.StudentName))
                    {
                        await _messageService.ShowAlert("You must enter your name before submitting");
                        return;
                    }
                    var result = await _gradeBookRepository.AddSubmission(Submission);
                    await _messageService.ShowAlert("Submitted!");
                    await _navigationService.ToPage("HomeView");
                }
                else
                {
                    var project = await _gradeBookRepository.AddProject(CodeProject);
                    await _navigationService.ToPage("ProjectPublishedView", new NavigationParameter() { { "Project", project } });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await _messageService.ShowAlert(e.ToString());
            }

        }

        public async Task Test()
        {
            try
            {
                var gradeCases = ConvertTextToGradeCases();

                var codes = await _visualStudioService.GetCSharpFilesAsync();
                var result = await _grader.Grade(codes.Select(p => p.Content), gradeCases);

                Submission.Grade = result.PercentPassing;
                ErrorMessage = "";
                ActualOutput = "";
                foreach (var resultCaseResult in result.CaseResults)
                {
                    if (!string.IsNullOrWhiteSpace(resultCaseResult.ErrorMessage))
                    {
                        ErrorMessage += resultCaseResult.ErrorMessage + "\r\n";
                    }

                    if (resultCaseResult.ActualOutput.Any())
                    {
                        this.ActualOutput += resultCaseResult.ActualOutput.Aggregate((f, s) => f + "," + s) + "\r\n";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorMessage = e.Message;
            }

        }

        public List<IGradeCase> ConvertTextToGradeCases()
        {
            var inputs =
                CsvCases?.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries) ??
                new string[0];
            var outputs =
                CsvExpectedOutput?.Split(new string[] { Environment.NewLine },
                    StringSplitOptions.RemoveEmptyEntries) ?? new string[0];
            var gradeCases = ConvertToGradeCases(outputs, inputs);
            return gradeCases;
        }

        public static List<IGradeCase> ConvertToGradeCases(string[] outputs, string[] inputs)
        {
            var gradeCases = new List<IGradeCase>();

            var highest = new[] { outputs.Length, inputs.Length }.Max();

            for (var index = 0; index < highest; index++)
            {
                var output = "";
                var input = "";
                if (index < inputs.Length)
                {
                    input = inputs[index];
                }

                if (index < outputs.Length)
                {
                    output = outputs[index];
                }



                //string[] inputArray = new string[0];
                //if (!string.IsNullOrEmpty(input))
                //{
                //    inputArray = input.Split(new[] {","}, StringSplitOptions.None);
                //}

                //string[] outputArray = new string[0];

                //if (!string.IsNullOrEmpty(output))
                //{
                //    outputArray = output.Split(new[] {","}, StringSplitOptions.None);
                //}

                gradeCases.Add(new GradeCase(input, output, index + 1));
            }

            if (gradeCases.Any() != true)
            {
                gradeCases.Add(new GradeCase(1));
            }

            return gradeCases;
        }


        public string ErrorMessage
        {
            get => _errorMessage;
            private set => SetProperty(ref _errorMessage, value);
        }



        public string ActualOutput
        {
            get => _actualOutput;
            private set => SetProperty(ref _actualOutput, value);
        }

        public DelegateCommandAsync TestCommand { get; set; }
        public DelegateCommand SubmitCommand { get; set; }

        public Submission Submission
        {
            get => _submission;
            set => SetProperty(ref _submission, value);
        }

        public ObservableCollection<IGradeCase> Cases
        {
            get => _cases;
            set => SetProperty(ref _cases, value);
        }

        public bool IsShowingCode
        {
            get => _isShowingCode;
            set => SetProperty(ref _isShowingCode, value);
        }
    }
}