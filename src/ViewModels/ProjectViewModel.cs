using System;
using AsyncToolWindowSample.Models;
using Grader;

namespace AsyncToolWindowSample.ToolWindows
{
    
    public class ProjectViewModel : BindableBase
    {
        private readonly IVisualStudioService _visualStudioService;
        private readonly ConsoleAppGrader _grader;
        private string _inputCases;
        private string _expectedOutput;
        private string _actualOutput;
        private double _percentPass;
        private string _errorMessage;

        public ProjectViewModel(IVisualStudioService visualStudioService, ConsoleAppGrader grader)
        {
            _visualStudioService = visualStudioService;
            _grader = grader;
            InputCases = "";
            ExpectedOutput = "Hello World!";

            TestCommand = new DelegateCommand(Test);
            SubmitCommand = new DelegateCommand(Submit);
        }

        private void Submit()
        {
            
        }

        private async void Test()
        {
            var codes = await _visualStudioService.GetCSharpFilesAsync();
            var cases = InputCases.Split(new string[]{Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
            var result = await _grader.Grade(codes, )
        }

        public double PercentPass
        {
            get => _percentPass;
            set => SetProperty(ref _percentPass,value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage,value);
        }

        public string InputCases
        {
            get => _inputCases;
            set => SetProperty(ref _inputCases,value);
        }

        public string ExpectedOutput
        {
            get => _expectedOutput;
            set => SetProperty(ref _expectedOutput,value);
        }

        public string ActualOutput
        {
            get => _actualOutput;
            set => SetProperty(ref _actualOutput,value);
        }

        public DelegateCommand TestCommand { get; set; }
        public DelegateCommand SubmitCommand { get; set; }
    }
}