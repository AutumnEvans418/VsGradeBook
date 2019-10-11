using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AsyncToolWindowSample.ToolWindows;
using Microsoft.Win32;

namespace Grader.Views
{

    public class InputBoxService : IMessageService
    {
        public async Task<string> ShowInputBox(string title, string msg, Func<string> importFunc)
        {
            var win = new InputBox(msg, importFunc);
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

    /// <summary>
    /// Interaction logic for InputBox.xaml
    /// </summary>
    public partial class InputBox : Window
    {
        private readonly Func<string> _importFunc;

        public InputBox(string msg, Func<string> importFunc)
        {
            _importFunc = importFunc;
            InitializeComponent();
            this.Label.Content = msg;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ImportClick(object sender, RoutedEventArgs e)
        {
            if (_importFunc != null)
                this.TextBox.Text = _importFunc();
        }
    }
}
