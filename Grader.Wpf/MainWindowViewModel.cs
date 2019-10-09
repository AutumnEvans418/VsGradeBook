using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncToolWindowSample;
using AsyncToolWindowSample.Models;

namespace Grader.Wpf
{
    public class MainWindowViewModel : BindableBase, IVisualStudioService
    {
        private string _code;

        public string Code
        {
            get => _code;
            set => SetProperty(ref _code, value);
        }

        public MainWindowViewModel()
        {
            Code = @"
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(""Hello World!"");
        }
    }
}
";
        }

        public async Task<IEnumerable<FileContent>> GetCSharpFilesAsync()
        {
            return new[] {new FileContent(){Content = Code, FileName = "Program.cs"}, };
        }

        public void OpenOrCreateCSharpFile(string fileName, string content)
        {
            Code = content;
        }
    }
}