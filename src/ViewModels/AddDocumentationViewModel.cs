using System;
using System.Windows;
using Grader;
using Console = System.Console;

namespace AsyncToolWindowSample.ToolWindows
{
    public class AddDocumentationViewModel : BindableBase
    {
        private readonly Action _exit;
        private readonly string _path;
        private readonly TextViewSelection _selection;
        private readonly ICSharpGenerator _cSharpGenerator;
        private string _code;
        private string _documentation;

        public string Code
        {
            get => _code;
            set => SetProperty(ref _code, value);
        }

        public string Documentation
        {
            get => _documentation;
            set => SetProperty(ref _documentation, value);
        }

        public DelegateCommand SaveCommand { get; }
        public DelegateCommand CancelCommand { get; }

        public AddDocumentationViewModel(Action exit, string path, TextViewSelection selection, ICSharpGenerator cSharpGenerator)
        {
            _exit = exit;
            _path = path;
            _selection = selection;
            _cSharpGenerator = cSharpGenerator;
            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(Cancel);
            Code = selection.Text;
            Grade();
        }

        private async void Grade()
        {
            var grader = new ConsoleAppGrader(_cSharpGenerator);
            try
            {
                await grader.Grade(Code, new[] { new GradeCase(), });
            }
            catch (CompilationException e)
            {
                Console.WriteLine(e);
                MessageBox.Show(e.Message);
            }
        }

        private void Cancel()
        {
            _exit();
        }

        private void Save()
        {

        }
    }
}