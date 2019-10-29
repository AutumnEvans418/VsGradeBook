using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using AsyncToolWindowSample.ToolWindows;
using Microsoft.Win32;
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace Grader.Views
{
    public class InputBoxService : IMessageService
    {
        private readonly IColorService _colorService;

        public InputBoxService(IColorService colorService)
        {
            _colorService = colorService;
        }
        public async Task<string> ShowInputBox(string title, string msg, Func<string> importFunc)
        {
            var win = new InputBox(msg, importFunc);
            _colorService.SetTheme(win);
            win.Title = title;
            win.ShowDialog();
            return win.TextBox.Text;
        }

        public async Task ShowAlert(string msg)
        {
            MessageBox.Show(msg);
        }


        public string ShowOpenDialog()
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Json Files|*.json";
            dialog.FileName = "project";
            if (dialog.ShowDialog() == true)
            {
                return File.ReadAllText(dialog.FileName);
            }
            return null;
        }

        public async Task ShowSaveDialog(string content)
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "Json Files|*.json";
            dialog.FileName = "project";
            if (dialog.ShowDialog() == true)
            {
                File.WriteAllText(dialog.FileName, content);
            }
        }
    }
}