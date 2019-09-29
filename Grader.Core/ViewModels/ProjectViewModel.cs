using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AsyncToolWindowSample.Models;
using Grader;
using Console = System.Console;

namespace AsyncToolWindowSample.ToolWindows
{
    public class ProjectViewModel : BindableBase
    {
        private readonly IVisualStudioService _visualStudioService;
        private readonly IConsoleAppGrader _grader;
        private string _inputCases;
        private string _expectedOutput;
        private string _actualOutput;
        private double _percentPass;
        private string _errorMessage;
        private CodeProject _codeProject;

        public ProjectViewModel(IVisualStudioService visualStudioService, IConsoleAppGrader grader)
        {
            _visualStudioService = visualStudioService;
            _grader = grader;
            InputCases = "";
            ExpectedOutput = "Hello World!";

            TestCommand = new DelegateCommandAsync(Test);
            SubmitCommand = new DelegateCommand(Submit);
        }

        public CodeProject CodeProject
        {
            get => _codeProject;
            set => SetProperty(ref _codeProject,value);
        }

        private void Submit()
        {

        }

        public async Task Test()
        {
            try
            {
                var gradeCases = new List<IGradeCase>();
                var codes = await _visualStudioService.GetCSharpFilesAsync();
                var inputs = InputCases.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                var outputs = ExpectedOutput.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                for (var index = 0; index < outputs.Length; index++)
                {
                    var input = "";
                    if (index < inputs.Length)
                    {
                        input = inputs[index];
                    }

                    var output = outputs[index];


                    string[] inputArray = new string[0];
                    if (!string.IsNullOrEmpty(input))
                    {
                        inputArray = input.Split(new[] { "," }, StringSplitOptions.None);
                    }

                    string[] outputArray = new string[0];

                    if (!string.IsNullOrEmpty(output))
                    {
                        outputArray = output.Split(new[] { "," }, StringSplitOptions.None);
                    }

                    gradeCases.Add(new GradeCase(inputArray, outputArray));
                }


                var result = await _grader.Grade(codes, gradeCases);

                PercentPass = result.PercentPassing;
                ErrorMessage = "";
                ActualOutput = "";
                foreach (var resultCaseResult in result.CaseResults)
                {
                    ErrorMessage += resultCaseResult.ErrorMessage + "\r\n";
                    if (resultCaseResult.ActualOutput.Any())
                        this.ActualOutput += resultCaseResult.ActualOutput.Aggregate((f, s) => f + "," + s) + "\r\n";

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                ErrorMessage = e.Message;
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
            private set => SetProperty(ref _errorMessage, value);
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
            private set => SetProperty(ref _actualOutput, value);
        }

        public DelegateCommandAsync TestCommand { get; set; }
        public DelegateCommand SubmitCommand { get; set; }
    }
}