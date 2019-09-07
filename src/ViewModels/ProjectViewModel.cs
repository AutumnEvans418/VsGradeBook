using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            TestCommand = new DelegateCommandAsync(Test);
            SubmitCommand = new DelegateCommand(Submit);
        }

        private void Submit()
        {

        }

        public async Task Test()
        {
            var gradeCases = new List<IGradeCase>();
            var codes = await _visualStudioService.GetCSharpFilesAsync();
            var inputs = InputCases.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var outputs = ExpectedOutput.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            for (var index = 0; index < outputs.Length; index++)
            {
                var input = "";
                if (index < inputs.Length)
                {
                    input = inputs[index];
                }
                var output = outputs[index];
                gradeCases.Add(new GradeCase(input.Split(new[] { "," }, StringSplitOptions.None), output.Split(new[] { "," }, StringSplitOptions.None)));
            }


            var result = await _grader.Grade(codes, gradeCases);

            PercentPass = result.PercentPassing;
            ErrorMessage = "";
            ActualOutput = "";
            foreach (var resultCaseResult in result.CaseResults)
            {
                ErrorMessage += resultCaseResult.Message + "\r\n";
                if (resultCaseResult.ActualOutput.Any())
                    this.ActualOutput += resultCaseResult.ActualOutput.Aggregate((f, s) => f + "," + s) + "\r\n";
            }
        }

        public double PercentPass
        {
            get => _percentPass;
            set => SetProperty(ref _percentPass, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public string InputCases
        {
            get => _inputCases;
            set => SetProperty(ref _inputCases, value);
        }

        public string ExpectedOutput
        {
            get => _expectedOutput;
            set => SetProperty(ref _expectedOutput, value);
        }

        public string ActualOutput
        {
            get => _actualOutput;
            set => SetProperty(ref _actualOutput, value);
        }

        public DelegateCommandAsync TestCommand { get; set; }
        public DelegateCommand SubmitCommand { get; set; }
    }
}